namespace Aras.Core.Tests.Setup {
    public class UserSession {

        public string Name { get; private set; }
        public Innovator.Client.IOM.Innovator Inn { get; private set; }

        public UserSession(string name, Innovator.Client.IOM.Innovator inn) {
            Name = name;
            Inn = inn;
        }
    }
}
