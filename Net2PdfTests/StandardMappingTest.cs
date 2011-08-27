using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net2Pdf;

namespace Net2PdfTests
{
    [TestClass]
    public class StandardMappingTest
    {
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

        private TestObj InitializeMocks()
        {
            return TestObj.InitializeMocks();
        }

        private Dictionary<string,string> GetMappings(object obj)
        {
          return  new PropertyMapper(new StringFormatterCollection()).GetMappings(obj);
        }

        [TestMethod]
        public void BasicPropertiesTest()
        {
            TestObj x1 = InitializeMocks();
            var propertyNameAndValue = GetMappings(x1);

            Assert.AreEqual("10", propertyNameAndValue["MyInt"]);
            Assert.AreEqual("I'm A String", propertyNameAndValue["MyString"]);
        }

        [TestMethod]
        public void DateTimeTest()
        {
            TestObj x1 = InitializeMocks();
            var propertyNameAndValue = GetMappings(x1);
            Assert.AreEqual(new DateTime(2011, 1, 1, 12, 1, 1).ToString(), propertyNameAndValue["MyDateTime"]);
        }

        [TestMethod]
        public void NestedClassesTest()
        {
            TestObj x1 = InitializeMocks();
            var propertyNameAndValue = GetMappings(x1);

            Assert.AreEqual("I'm a Sub String", propertyNameAndValue["Sub.SubString"]);
            Assert.AreEqual("99", propertyNameAndValue["Sub.Inner.InnerInt"]);
            Assert.AreEqual(new DateTime(2011, 2, 2, 12, 2, 2).ToString(), propertyNameAndValue["Sub.SubDateTime"]);
        }

        [TestMethod]
        public void EnumTest()
        {
            TestObj x1 = InitializeMocks();
            var propertyNameAndValue = GetMappings(x1);

            Assert.AreEqual("True", propertyNameAndValue["MyEnum.e1"]);
            Assert.AreEqual("False", propertyNameAndValue["MyEnum.e2"]);

            Assert.AreEqual("e1", propertyNameAndValue["MyEnum"]);
        }
        
        [TestMethod]
        public void BoolTest()
        {
            TestObj x1 = InitializeMocks();
            var propertyNameAndValue = GetMappings(x1);

            Assert.AreEqual("True", propertyNameAndValue["MyBool"]);
        }

        //[TestMethod]
        //public void ListTest()
        //{
        //    TestObj x1 = InitializeMocks();
        //    var propertyNameAndValue = GetMappings(x1);
        //    Assert.AreEqual("a", propertyNameAndValue["MyList[1]"]);
        //}

        [TestMethod]
        public void ComplexListTest()
        {
            TestObj x1 = InitializeMocks();
            var propertyNameAndValue = GetMappings(x1);

            Assert.AreEqual("List String 1", propertyNameAndValue["MyComplexList[0].SubString"]);
        }
    }
}
