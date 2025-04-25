using Innovator.Client.IOM;
using Aras.Core.Tests.Common;
using Aras.Core.Tests;
using Aras.Core.Tests.Models;
using Aras.Core.Tests.ArasExtensions;

namespace Aras.OOTB.Tests.Models {
    internal class Document :
        ICreateNew
    {
        private const string ITEM_TYPE = "Document";

        public Item CreateNew(Innovator.Client.IOM.Innovator inn) {
            Item item = inn.newItem(ITEM_TYPE, "add");
            string itemNumber = Generators.GetNewId();;
            item.setProperty("item_number", itemNumber);
            item = item.apply();
            return item;
        }

        public Item AddFile(Innovator.Client.IOM.Innovator inn, Item document, string filePath) {
            var conn = inn.getConnection();
            var upload = conn.CreateUploadCommand();
            string newId = inn.getNewID();
            upload.AddFile(newId, filePath);
                        
            Stream result = conn.Process(upload);
            
            Item fileItem = inn.getItemById("File", newId, "id");
            return fileItem;    
        }


    }
}
