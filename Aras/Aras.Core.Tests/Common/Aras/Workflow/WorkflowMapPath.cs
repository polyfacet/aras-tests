namespace Aras.Core.Tests.Workflow;

public class WorkflowMapPath {

    private Innovator.Client.IOM.Innovator Inn;
    private Item _workflowMapPathItem;
    public WorkflowMapPath(Item workflowMapPathItem)
    {
        _workflowMapPathItem = workflowMapPathItem;
        Inn = _workflowMapPathItem.getInnovator();
        Name = _workflowMapPathItem.getProperty("name");
        string sourceId = _workflowMapPathItem.getProperty("source_id");
        string relatedId = _workflowMapPathItem.getProperty("related_id");       
        Item sourceItem = Inn.GetItemById("Activity Template", sourceId);
        Item relatedItem = Inn.GetItemById("Activity Template", relatedId);
        FromActivity = new(sourceItem);
        ToActivity = new(relatedItem);
    }

    public readonly string Name;
    public readonly ActivityTemplate FromActivity;
    public readonly ActivityTemplate ToActivity;

}