using Innovator.Client.IOM;

namespace Aras.OOTB.Tests.LoadTests.UserScenario;

public interface IUserScenario
{
    string Description { get; }
    Item Run(Innovator.Client.IOM.Innovator inn);
    
}