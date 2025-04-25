using Aras.Core.Tests;
using Aras.Core.Tests.ArasExtensions;
using Innovator.Client.IOM;

namespace Aras.OOTB.Tests.LoadTests.UserScenario;

public class GetPart : IUserScenario {

    string IUserScenario.Description => "Get Part";
    public Item Run(Innovator.Client.IOM.Innovator inn) {
        Item part = GetLastestCreatedPart(inn);
        return part;
    }

    private Item GetLastestCreatedPart(Innovator.Client.IOM.Innovator inn) {
        Item part = inn.newItem("Part", "get");
        part.setAttribute("maxRecords", "1");
        part.setAttribute("orderBy", "created_on DESC");
        part = part.Apply();
        return part;
    }
    
}