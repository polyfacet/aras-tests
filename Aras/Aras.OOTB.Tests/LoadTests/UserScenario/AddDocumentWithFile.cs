using Aras.OOTB.Tests.Models;
using Innovator.Client.IOM;

namespace Aras.OOTB.Tests.LoadTests.UserScenario;

public class AddDocumentWithFile : IUserScenario {
    string IUserScenario.Description => "Add Document With File";

    public Item Run(Innovator.Client.IOM.Innovator inn) {
        Document document = new Document();
        Item doc = document.CreateNew(inn);
        string filePath = CreateTestFile();
        Item fileItem = document.AddFile(inn, doc, filePath);
        LinkFileToDoc(doc, fileItem);
        return doc;
    }

    private Item LinkFileToDoc(Item doc, Item fileItem) {
        Innovator.Client.IOM.Innovator inn = doc.getInnovator();
        Item rel = inn.newItem("Document File", "add");
        rel.setProperty("source_id", doc.getID());
        rel.setProperty("related_id", fileItem.getID());
        return rel.apply();
    }

    private string CreateTestFile() {
        string fileName = "TestFile.txt";
        string filePath = Path.Combine(Path.GetTempPath(), fileName);
        using (StreamWriter sw = new StreamWriter(filePath)) {
            sw.WriteLine("Test");
        }
        return filePath;
    }

 }