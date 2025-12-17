
namespace Aras.Core.Tests.Workflow;

public class WorkflowMap 
{
    private Innovator.Client.IOM.Innovator Inn;
    private string _workflowMapId;

    private Item _workflowMapItem;

    private List<ActivityTemplate>? _activityTemplates;
    public WorkflowMap(Innovator.Client.IOM.Innovator inn, string workflowMapId)
    {
        Inn = inn;
        _workflowMapId = workflowMapId;
        _workflowMapItem = inn.GetItemById("Workflow Map", workflowMapId);
    }
  
    public List<ActivityTemplate> ActivityTemplates 
    { 
        get
        {   
            if (_activityTemplates == null) {
                _activityTemplates = FetchActivityTemplates();
            } 
            return _activityTemplates;
        }
    }

    private List<ActivityTemplate> FetchActivityTemplates()
    {
        List<ActivityTemplate> activityTemplates = new ();

        string aml = $@"<AML>
            <Item action='get' type='Workflow Map Activity' select='sort_order, related_id' >
                <source_id>{_workflowMapId}</source_id>
                <related_id>
                <Item type='Activity Template' action='get' select='name, is_end, is_start'>
                    <Relationships>
                    <Item action='get' type='Workflow Map Path' select='name, related_id'>
                        <!-- related_id = Activity Template -->
                    </Item>          
                    </Relationships>
                </Item>
                </related_id>
            </Item>
            </AML>";
        Item rels = Inn.applyAML(aml);
        for(int i = 0; i<rels.getItemCount(); i++) {
            Item activityTemplateItem = rels.getItemByIndex(i).getRelatedItem();
            ActivityTemplate activityTemplate = new(activityTemplateItem);
            activityTemplates.Add(activityTemplate);
        }

        return activityTemplates;
    }
}