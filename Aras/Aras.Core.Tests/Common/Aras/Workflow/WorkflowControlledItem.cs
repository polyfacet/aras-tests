namespace Aras.Core.Tests.Workflow;

public abstract class WorkflowControlledItem : InnovatorBase
{
    public readonly Item SourceItem;
    public WorkflowControlledItem(Item sourceItem) : base(sourceItem.getInnovator())
    {
        SourceItem = sourceItem;
    }

    public void ClaimSignOffs(string votePath, Innovator.Client.IOM.Innovator claimTo)
    {
        foreach(Item activityAssignments in GetActivityAssignments(votePath)){
            string activityAssignmentId = activityAssignments.getID();
            string toUserIdentityId = claimTo.GetIdentity().getID();
            string amlUpdate = $@"<AML>
                <Item action='edit' type='Activity Assignment' id='{activityAssignmentId}'>
                    <related_id>{toUserIdentityId}</related_id>
                </Item></AML>";  
            Item res = claimTo.applyAML(amlUpdate);
        }
    }

    public Item SignOff(string votePath, bool tickInAllRequiredTasks = false)
    {
        return SignOffAs(votePath, Inn, tickInAllRequiredTasks);
    }
    public Item SignOffAndAssert(string votePath, bool tickInAllRequiredTasks = false)
    {
        Item result =  SignOffAs(votePath, Inn, tickInAllRequiredTasks);
        AssertItem.IsNotError(result);
        return result;
    }

    public Item ClaimAndSignOffAndAssert(string votePath, Innovator.Client.IOM.Innovator claimTo, bool tickInAllTasks = false)
    {
        ClaimSignOffs(votePath, claimTo);
        Item result = SignOff(votePath, tickInAllTasks);
        AssertItem.IsNotError(result);
        return result;
    }

    public Item SignOffAs(string votePath, Innovator.Client.IOM.Innovator signOfAsInn, bool tickInAllRequiredTasks = false, bool doAssert = false) {
        Item currentUserIdentity = signOfAsInn.GetIdentity();
        Item activeWorkFlow = GetActiveWorkflow();
        if (activeWorkFlow.isError()) return activeWorkFlow;
        List<Item> activeActivities = Common.Aras.Workflow.GetActiveActivities(activeWorkFlow, votePath);
        foreach (Item activity in activeActivities) {
            Item assignments = activity.getRelationships("Activity Assignment");
            Item activeActivity = Common.Aras.Workflow.GetActiveActivity(Inn, activity.getID(),votePath);
            Item paths = activeActivity.getRelationships("Workflow Process Path");
            if (paths.getItemCount() < 1) continue;
            string pathId = paths.getItemByIndex(0).getID(); //Can we get more than one?
            for (int i = 0; i < assignments.getItemCount(); i++) {
                Item assignment = assignments.getItemByIndex(i);
                string assignmentId = assignment.getID();
                assignment = signOfAsInn.getItemById("Activity Assignment", assignmentId); // Need to make a clean load of the item
                Item assignmentIdentity = assignment.getRelatedItem();
                Common.Aras.Users user = new Common.Aras.Users(signOfAsInn);
                if (user.IsDirectMemberOf(assignmentIdentity) ||
                    currentUserIdentity.getID().Equals(assignmentIdentity.getID()) ) {
                    
                    List<string> requiredActivityTaskIds = new();
                    if (tickInAllRequiredTasks) {
                        requiredActivityTaskIds = GetRequiredActivityTaskIds(assignment);
                    }
                    // Vote
                    Item result = Common.Aras.Workflow.ApplyVote(signOfAsInn, activity.getID(), assignmentId, pathId, ArasTestBase.TEST_NAME, requiredActivityTaskIds);
                    if (doAssert) {
                        AssertItem.IsNotError(result);
                    }
                    return result;
                }
            }
        }
        return Inn.newError($"Nothing to signoff with '{votePath}' was found");
    }

    private List<string> GetRequiredActivityTaskIds(Item assignment)
    {
        List<string> requiredTaskIds = new();

        string aml = $@"<AML>
            <Item action='get' type='Activity Task Value' select='task'  >
                <source_id>{assignment.getID()}</source_id>
                <task>
                <Item type='Activity Task' action='get'>
                    <is_required>1</is_required>           
                </Item>  
                </task>
            </Item>
            </AML>";
        Item activityTaskValues = Inn.applyAML(aml);
        for (int i = 0; i < activityTaskValues.getItemCount(); i++)
        {
            Item activityTaskValue = activityTaskValues.getItemByIndex(i);
            string taskId = activityTaskValue.getProperty("task");
            requiredTaskIds.Add(taskId);
        }
        return requiredTaskIds;        
    }

    public bool IsAssignedTo(Item identity)
    {
        List<Item> assignedToIdentities = AssignedToIdentities();
        foreach(Item assignedToIdentity in assignedToIdentities) {
            if (assignedToIdentity.getID() == identity.getID()) return true;
        }
        return false;
    }


    public List<Item> AssignedToIdentities()
    {
        List<Item> assignedTo = new ();
        List<Item> activityAssignments = GetActiveActivityAssignments();
        foreach (Item assignment in activityAssignments) {
            assignedTo.Add(assignment.getRelatedItem());
        }
        return assignedTo;
    }

    public List<Item> GetActiveActivityAssignments() {
        List<Item> activityAssignments = new ();
        List<Item> activeActivities = Common.Aras.Workflow.GetActiveActivities(GetActiveWorkflow());
        foreach (Item activity in activeActivities) {
            Item assignmentsRels = activity.getRelationships("Activity Assignment");
            for (int i = 0;  i < assignmentsRels.getItemCount(); i++) {
                Item assignment = assignmentsRels.getItemByIndex(i);
                activityAssignments.Add(assignment);
            }
        }
        return activityAssignments;
    }

    public List<Item> GetActiveActivities() { 
        Item activeWorkFlow = GetActiveWorkflow();
        if (activeWorkFlow.isError()) return new List<Item>();
        return Common.Aras.Workflow.GetActiveActivities(activeWorkFlow);
    }

    /// <summary>
    /// Checks/wait for activities starting with "Processing"
    /// </summary>
    /// <param name="waitTimeSeconds"></param>
    /// <param name="maxWaitLoops"></param>
    /// <returns>False if still has "Processing" activity after 'timeout'</returns>
    public bool WaitForProcessingActivities(int waitTimeSeconds = 2, int maxWaitLoops = 60)
    {
        for(int i = 0; i<maxWaitLoops; i++) {
            System.Threading.Thread.Sleep(waitTimeSeconds*1000);
            List<Item> activities = this.GetActiveActivities();
            if (activities.Count == 0 ) return true; // No active activities, assume completed
                
            bool hasProcessingActivity = false;
            foreach (Item activity in activities)
            {
                string label = activity.getProperty("label");
                if (label.StartsWith("Processing")) hasProcessingActivity = true;
            }
            if (!hasProcessingActivity) return true;             
        }
        return false;
    }

    private List<Item> GetActivityAssignments(string votePath) {
        List<Item> activityAssignments = new List<Item> ();
        Item activeWorkFlow = GetActiveWorkflow();
        if (activeWorkFlow.isError()) return activityAssignments;
        List<Item> activeActivities = Common.Aras.Workflow.GetActiveActivities(activeWorkFlow, votePath);
        foreach (Item activity in activeActivities) {
            Item assignments = activity.getRelationships("Activity Assignment");
            Item activeActivity = Common.Aras.Workflow.GetActiveActivity(Inn, activity.getID(),votePath);
            Item paths = activeActivity.getRelationships("Workflow Process Path");
            if (paths.getItemCount() < 1) continue;
            string pathId = paths.getItemByIndex(0).getID(); //Can we get more than one?
            for (int i = 0; i < assignments.getItemCount(); i++) {
                Item assignment = assignments.getItemByIndex(i);
                string assignmentId = assignment.getID();
                assignment = SourceItem.getInnovator().getItemById("Activity Assignment", assignmentId); // Need to make a clean load of the item
                activityAssignments.Add(assignment);
            }
        }
        return activityAssignments;

    }

    private Item GetActiveWorkflow() {
            return Common.Aras.Workflow.GetActiveWorkflowProcess(SourceItem);
    }

}


