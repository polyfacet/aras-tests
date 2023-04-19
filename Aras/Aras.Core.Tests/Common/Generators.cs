
namespace Aras.Core.Tests.Common {
    public class Generators {

        public const string TEST_NAME = "AutoTest";
        public static string GetNewId(int length = 8) {
            string id = Guid.NewGuid().ToString();
            if (length > 32) return id;
            return id.Substring(0, length);
        }
    }
}
