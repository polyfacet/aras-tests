

namespace Aras.Core.Tests.Workflow;

public class ActivityTemplate 
{
    private Innovator.Client.IOM.Innovator Inn;
    private Item _activityTemplateItem;
    private string _activityTemplateId;
    private string _name;
    private string _label;

    private List<WorkflowMapPath>? _workflowMapPaths;
    
    public ActivityTemplate(Item activityTemplate)
    {
        _activityTemplateItem = activityTemplate;
        _activityTemplateId = _activityTemplateItem.getID();
        Inn = _activityTemplateItem.getInnovator();
        _name = _activityTemplateItem.getProperty("name");
        _label = _activityTemplateItem.getProperty("label");
        string isStart = _activityTemplateItem.getProperty("is_start");
        string isEnd = _activityTemplateItem.getProperty("is_end");
        if (AnyNullOrEmpty(_name, isStart, isEnd, _label)) {
            _activityTemplateItem = Inn.GetItemById("Activity Template" , _activityTemplateId);
            _name = _activityTemplateItem.getProperty("name");
            isStart = _activityTemplateItem.getProperty("is_start");
            isEnd = _activityTemplateItem.getProperty("is_end");
        } 
        IsStart = isStart == "1";
        IsEnd = isEnd == "1";
    }

    public string Name => _name;
    public string Label => _label;
    public bool IsStart;
    public bool IsEnd;

    public List<WorkflowMapPath> WorkflowMapPaths => _workflowMapPaths ??= FetchWorkflowMapPaths();
    

    private List<WorkflowMapPath> FetchWorkflowMapPaths()
    {
        string aml = $@"<AML>
            <Item action='get' type='Workflow Map Path' select='name, source_id, related_id'>
                <source_id>{_activityTemplateId}</source_id>
            </Item></AML>";
        Item workflowMapPathItems = Inn.applyAML(aml);
        _workflowMapPaths = new();
        for (int i = 0; i < workflowMapPathItems.getItemCount(); i++)
        {
            Item workflowMapPathItem = workflowMapPathItems.getItemByIndex(i);
            _workflowMapPaths.Add(new(workflowMapPathItem));
        }
        return _workflowMapPaths;
    }

    private static bool AnyNullOrEmpty(params string[] values) {
        foreach (string value in values)
            if (string.IsNullOrEmpty(value)) return true;
        return false;
    }
}