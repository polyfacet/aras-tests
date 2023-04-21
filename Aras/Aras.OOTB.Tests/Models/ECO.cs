using Innovator.Client.IOM;
using Common = Aras.Core.Tests.Common;
using Aras.Core.Tests;
using Aras.Core.Tests.ArasExtensions;
using System.Security.Cryptography.X509Certificates;

namespace Aras.OOTB.Tests.Models {
    internal class ECO : InnovatorBase        
    {
        private const string ITEM_TYPE = "Express ECO";
        private const string AFFECTED_ITEM = "Affected Item";
        private const string ECO_AFFECTED_ITEM = "Express ECO Affected Item";

        private readonly Item ECOItem;

        public enum AffectedItemAction {
            Release,
            Revise
        }

        public ECO(Item eco) : base(eco.getInnovator()) {
            ECOItem = eco;
        }

        /// <summary>
        /// Returns the Express ECO Affected Item
        /// </summary>
        /// <param name="changeControlledItem"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Item AddAffectedItem(Item changeControlledItem, AffectedItemAction action ) {
            string aml = $@"<AML>
              <Item type='{ECO_AFFECTED_ITEM}' action='add'>
                <related_id>
                  <Item type='{AFFECTED_ITEM}'  action='add' >
                    <item_action>{action}</item_action>
                    <new_item_id>{changeControlledItem.getID()}</new_item_id>
                  </Item>
                </related_id>
                <source_id>{ECOItem.getID()}</source_id>
              </Item> 
            </AML>";
            Item item = Inn.applyAML(aml);
            return item;
        }

        internal Item SignOff(string votePath) {
            Item currentUserIdentity = Inn.GetIdentity();
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
                    assignment = Inn.getItemById("Activity Assignment", assignmentId); // Need to make a clean load of the item
                    Item assignmentIdentity = assignment.getRelatedItem();
                    Common.Aras.Users user = new Common.Aras.Users(Inn);
                    if (user.IsDirectMemberOf(assignmentIdentity) ||
                        currentUserIdentity.getID().Equals(assignmentIdentity.getID()) ) {
                        // Vote
                        Item result = Common.Aras.Workflow.ApplyVote(Inn, activity.getID(), assignmentId, pathId, ArasTestBase.TEST_NAME);
                        return result;
                    }
                }
            }
            return Inn.newError("Nothing to signoff was found");
        }

        private Item GetActiveWorkflow() {
            return Common.Aras.Workflow.GetActiveWorkflowProcess(ECOItem);
        }
    }
}
