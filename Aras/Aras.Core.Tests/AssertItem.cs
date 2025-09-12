using System.Collections.Generic;
using Innovator.Client.IOM;
using Xunit;

namespace Aras.Core.Tests {
    public class AssertItem
    {
        
        public static void IsNotError(Item item) {
            Assert.NotNull(item);
            Assert.False(item.isError(), item.getErrorString());
        }

        public static void IsNotError(Item item, string message) {
            Assert.NotNull(item);
            Assert.False(item.isError(), message);
        }

        public static void IsError(Item item) {
            IsError(item, $"Expected error item. DOM: {item.dom.InnerXml}");
        }

        public static void IsError(Item item, string message) {
            Assert.NotNull(item);
            Assert.True(item.isError(), message);
        }

        public static void IsInState(Item item, string expectedState) {
            IsPropertyValue(item, "state", expectedState);
        }

        public static void IsPropertyValue(Item item , string property, string expectedValue)
        {
            AssertItem.IsNotError(item);
            string actualValue = item.getProperty(property, "N/A");
            Assert.Equal(expectedValue, actualValue);
        }

        public static void HasNoValidationWarnings(Item item, string relationshipName)
        {
            var validations = GetValidations(item, relationshipName);
            foreach (var validation in validations)
            {
                string validationType = validation.getProperty("validation_type", "");
                Assert.NotEqual("Warning", validationType);
            }
        }

        public static void HasValidationWarnings(Item item, string relationshipName, string validationDescriptionIncludes = "")
        {
            var validations = GetValidations(item, relationshipName);
            foreach (var validation in validations)
            {
                string validationType = validation.getProperty("validation_type", "");
                string validationDescription = validation.getProperty("description", "");
                if (String.IsNullOrEmpty(validationDescriptionIncludes)) {
                    if (validationType.Equals("Warning")) {
                        return;
                    } 
                }
                if (validationType.Equals("Warning") 
                    && validationDescription.Contains(validationDescriptionIncludes, StringComparison.OrdinalIgnoreCase)) {
                    return;
                }
            }

            string identity = item.getProperty("keyed_name", "");
            if (identity == "") identity = item.getID();
            string message = $"No warning validations for: {identity} ";
            if (!String.IsNullOrEmpty(validationDescriptionIncludes)) {
                message += $" including '{validationDescriptionIncludes}' in description";
            }
            Assert.True(false, message);
        }

        private static List<Item> GetValidations(Item item, string relationshipName) {
            List<Item> validations = new();
            Innovator.Client.IOM.Innovator inn = item.getInnovator();
            string aml = $@"<AML><Item action='get' type='{item.getType()}' id='{item.getID()}'>
                <Relationships><Item 
                    action='get' 
                    type='{relationshipName}'>
                    </Item>
                </Relationships>
                </Item></AML>";
            
            Item itemWithRels = inn.applyAML(aml);
            Item relations = itemWithRels.getRelationships(relationshipName);
            for (int i = 0; i<relations.getItemCount(); i++) {
                Item relatedItem = relations.getItemByIndex(i).getRelatedItem();
                validations.Add(relatedItem);
            }
            return validations;
        }
    }
}
