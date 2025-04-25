using Aras.OOTB.Tests.Models;
using Innovator.Client.IOM;

namespace Aras.OOTB.Tests.LoadTests.UserScenario;

public class AddPart : IUserScenario {
    string IUserScenario.Description => "Add Part";

    public Item Run(Innovator.Client.IOM.Innovator inn) {
        Part part = new Part();
        return part.CreateNew(inn);
    }

 }