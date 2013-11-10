using System.Linq;
using System.Collections.Generic;

using AspNetMvc_Infrastructure;
using GuestBook_Data;
using GuestBook_WebRole.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNetMvc_Infrastructure_Test
{
    /// <summary>
    /// Summary description for ModelConverterSpec
    /// </summary>
    [TestClass]
    public class ModelConverterSpec
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Can_convert_Entity_To_Mvc_Model()
        {
            // Arrange
            IEnumerable<GuestBookEntry> entities = new List<GuestBookEntry>( new [] {
                        new GuestBookEntry{GuestName = "John", Comment = "Hi, my name is John."},
                        new GuestBookEntry{GuestName = "Lisa", Comment = "Hi, my name is Lisa."},
                        new GuestBookEntry{GuestName = "Felice", Comment = "Hi, my name is Felice."}
                    });

            // Act
            IEnumerable<GuestBookEntryModel> mvcModels = entities.ToMvcModels<GuestBookEntry, GuestBookEntryModel>();

            // Assert
            Assert.IsTrue(mvcModels.Any());
            int index = 0;
            foreach (GuestBookEntryModel guestBookEntryModel in mvcModels)
            {
                Assert.AreEqual(guestBookEntryModel.Name, entities.ElementAt(index).GuestName);
                Assert.AreEqual(guestBookEntryModel.Comment, entities.ElementAt(index).Comment);
                index++;
            }
        }
    }
}
