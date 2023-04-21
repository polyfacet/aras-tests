using Aras.Core.Tests.ArasExtensions;
using Innovator.Client.IOM;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Aras.Core.Tests.Common.Aras {
    public class Workflow {

        public static Item GetActiveWorkflowProcess(Item sourceItem) {
            string aml = $@"<AML>
                    <Item action ='get' type='Workflow' select='related_id'>
                        <source_id>{sourceItem.getID()}</source_id>
                        <related_id>
                            <Item type ='Workflow Process'>
                                <state>Active</state>
                            </Item>
                        </related_id>
                    </Item>
                </AML>";
            Innovator.Client.IOM.Innovator inn = sourceItem.getInnovator();
            Item workflow = inn.applyAML(aml);
            if (workflow.isError()) return workflow;
            if (workflow.isCollection()) return inn.newError("Not a singel workflow related");
            string wfpId = workflow.getItemsByXPath("//Item[@type='Workflow Process']").getItemByIndex(0).getID();
            return inn.getItemById("Workflow Process", wfpId, "id");
        }

        public static List<Item> GetActiveActivitiesForSourceItem(Item sourceItem) {
            Item activeWorkFlow = GetActiveWorkflowProcess(sourceItem);
            if (activeWorkFlow.isError()) throw new ApplicationException(activeWorkFlow.getErrorString());  
            return GetActiveActivities(activeWorkFlow);
        }

        public static List<Item> GetActiveActivities(Item workflowProcess, string withVotePathName) {
            List<Item> activeActivities = new();
            string votePathCondition = string.Empty;
            if (!String.IsNullOrEmpty(withVotePathName)) {
                votePathCondition = $"<name>{withVotePathName}</name>";
            }
            string aml = $@"<AML>
                    <Item type = 'Workflow Process' action='get' id='{workflowProcess.getID()}' >
                        <Relationships>
                            <Item action ='get' type='Workflow Process Activity' select='related_id'>
                                <related_id>
                                    <Item type='Activity' action='get' select='id,state'>
                                        <state>Active</state>
                                        <Relationships>
                                            <Item action='get' type='Activity Assignment'>
                                                <closed_on condition='is null'></closed_on>
                                           </Item>
                                           <Item action='get' type='Workflow Process Path'>
                                                {votePathCondition}     
                                           </Item>
                                        </Relationships>
                                    </Item>
                                </related_id>
                            </Item>
                        </Relationships>
                    </Item>
                </AML>";

            Item res = workflowProcess.getInnovator().applyAML(aml);
            if (!res.isError()) {
                Item activities = res.getItemsByXPath("//Item[@type='Activity']");
                for (int i = 0; i < activities.getItemCount(); i++) {
                    Item activity = activities.getItemByIndex(i); //.getRelatedItem();
                    activeActivities.Add(activity);
                }
            }
            return activeActivities;
        }

        public static List<Item> GetActiveActivities(Item workflowProcess) {
            return GetActiveActivities(workflowProcess, String.Empty);
        }

        public static Item ApplyVote(Innovator.Client.IOM.Innovator inn,
            string activityId, 
            string assignmentId, 
            string pathId, 
            string comments) {
            
            //var AuthMode = "password";
            var body = "";
            body += "<Item type='Activity' action='EvaluateActivity'>";
            body += "<Activity>" + activityId + "</Activity>";
            body += "<ActivityAssignment>" + assignmentId + "</ActivityAssignment>";

            body += "<Paths>";
            body += "<Path id='" + pathId + "'></Path>";
            body += "</Paths>";
            body += "<Tasks/>";
            body += "<Variables/>";
            body += "<Authentication mode=''/>";
            //body += "<Authentication mode='" + AuthMode + "'>" + pwdHash + "</Authentication>";
            body += "<Comments>" + comments + "</Comments>";
            body += "<Complete>1</Complete>";
            body += "</Item>";

            var aml = "<AML>";
            aml += body;
            aml += "</AML>";
            
            return inn.applyAML(aml);
        }

        public static Item GetActiveActivity(Innovator.Client.IOM.Innovator inn, string activityId, string withVotePathName) {
            string votePathCondition = string.Empty;
            if (!String.IsNullOrEmpty(withVotePathName)) {
                votePathCondition = $"<name>{withVotePathName}</name>";
            }
            string aml = $@"<AML>
                <Item type='Activity' action='get' select='id,state' id='{activityId}'>
                    <state>Active</state>
                    <Relationships>
                        <Item action='get' type='Activity Assignment'>
                            <closed_on condition='is null'></closed_on>
                        </Item>
                        <Item action='get' type='Workflow Process Path'>
                            {votePathCondition}
                        </Item>
                    </Relationships>
                </Item>
                </AML>";
            return inn.applyAML(aml);
        }
    }
}
