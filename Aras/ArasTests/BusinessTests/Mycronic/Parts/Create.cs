using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Innovator.Client.IOM;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace ArasTests.Mycronic.Parts
{
    internal class Create : InnovatorBase
    {
        private Dictionary<string, string> MandatoryPropertiesDefaultsMap = new Dictionary<string, string>
        {
            { "classification" ,"Designed part" },
            { "my_business_area" ,"Pattern Generator" },
            { "my_production_site" ,"Täby" },
            { "my_name" , "5F4199AA3D7946DF9B7345CAB1E665E9" }, // Test MY Dictionary Item
            { "my_qualifier" ,"Test" }
        };

        public Create(Innovator.Client.IOM.Innovator inn) : base(inn) {
        }

        public Item NewDesignedPart()
        {
            Item part = Inn.newItem("Part", "add");
            part.setProperty("item_number", GenerateTestItemNumber());
            foreach (var kvp in MandatoryPropertiesDefaultsMap) {
                part.setProperty(kvp.Key, kvp.Value);
            }
            part.setPropertyAttribute("my_name", "keyed_name", "Test");
            return part.apply();
        }

        private object GenerateTestItemNumber() {
            return "TEST- " + Guid.NewGuid().ToString().Substring(0, 8);
        }
    }
}
