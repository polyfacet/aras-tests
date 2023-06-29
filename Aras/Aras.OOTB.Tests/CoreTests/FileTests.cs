using Innovator.Client.IOM;
using Xunit.Abstractions;
using Aras.Core.Tests.ArasExtensions;
using Aras.OOTB.Tests.Fixture;

namespace Aras.OOTB.Tests.CoreTests;

public class FileTests : OOTB.Tests.OOTBTest
{
        public FileTests(DefaultArasSessionFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {}

        private const string ITEM_TYPE = "File";


        [Fact]
        [Trait("SmokeTest", "2")]
        public void Admin_can_add_a_File_to_vault() {
            // Arrange
            string fileName = "TestFile.txt";
            string filePath = Path.Combine(Path.GetTempPath(), fileName);
            using (StreamWriter sw = new StreamWriter(filePath)) {
                sw.WriteLine("Test");
            }

            var conn = AdminInn.getConnection();
            var upload = conn.CreateUploadCommand();
            string newId = AdminInn.getNewID();
            upload.AddFile(newId, filePath);
            
            // Act            
            Stream result = conn.Process(upload);
            
            // Assert
            Item fileItem = AdminInn.getItemById(ITEM_TYPE, newId, "id");
            AssertItem.IsNotError(fileItem);
            
        }
}