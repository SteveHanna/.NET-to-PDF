using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Net2Pdf;

namespace Net2PdfTests
{
    [TestClass]
    public class FormatterTests
    {
        private TestObj InitializeMocks()
        {
            return TestObj.InitializeMocks();
        }

        [TestMethod]
        public void BasicTypeTest()
        {
            TestObj x1 = InitializeMocks();
            var x = new StringFormatterCollection();
            x.Add(new ExpressionFormatter<TestObj>(o => o.MyInt + " set from unit test"));

            var propertyNameAndValues = new PropertyMapper(x).GetMappings(x1);
            Assert.AreEqual("10 set from unit test", propertyNameAndValues["MyInt"]);
        }

        [TestMethod]
        public void DateTime_Formatting_Test()
        {
            TestObj x1 = InitializeMocks();
            var x = new StringFormatterCollection();
            x.Add(new ExpressionFormatter<TestObj>(o => o.MyDateTime.ToShortDateString()));

            var propertyNameAndValues = new PropertyMapper(x).GetMappings(x1);
            Assert.AreEqual(new DateTime(2011, 1, 1, 12, 1, 1).ToShortDateString(), propertyNameAndValues["MyDateTime"]);
        }

        [TestMethod]
        public void Primitive_As_Root_Test()
        {
            TestObj x1 = InitializeMocks();
            var x = new StringFormatterCollection();
            x.Add(new ExpressionFormatter<DateTime>(o => o.ToShortDateString()));

            var propertyNameAndValues = new PropertyMapper(x).GetMappings(x1);
            Assert.AreEqual(new DateTime(2011, 1, 1, 12, 1, 1).ToShortDateString(), propertyNameAndValues["MyDateTime"]);
            Assert.AreEqual(new DateTime(2011, 2, 2, 12, 2, 2).ToShortDateString(), propertyNameAndValues["Sub.SubDateTime"]);
        }
        
        [TestMethod]
        public void Primitive_As_Root2_Test()
        {
            TestObj x1 = InitializeMocks();
            var x = new StringFormatterCollection();
            x.Add(new ExpressionFormatter<string>(o => o.ToUpper()));

            var propertyNameAndValues = new PropertyMapper(x).GetMappings(x1);
            
            Assert.AreEqual("I'M A STRING", propertyNameAndValues["MyString"]);
            Assert.AreEqual("I'M A SUB STRING", propertyNameAndValues["Sub.SubString"]);
            //Assert.AreEqual("I'M", propertyNameAndValues["MyList[0]"]);
        }

        [TestMethod]
        public void Multiple_Formats_Test()
        {
            TestObj x1 = InitializeMocks();
            var x = new StringFormatterCollection();
            x.Add(new ExpressionFormatter<TestObj>(o => o.MyString + " I'm the exception", o => o.MyDateTime.ToShortTimeString()));
            x.Add(new ExpressionFormatter<string>(o => o + " hello world"));

            var propertyNameAndValues = new PropertyMapper(x).GetMappings(x1);
            Assert.AreEqual(new DateTime(2011, 1, 1, 12, 1, 1).ToShortTimeString(), propertyNameAndValues["MyDateTime"]);
            Assert.AreEqual("I'm A String I'm the exception", propertyNameAndValues["MyString"]);
            Assert.AreEqual("I'm a Sub String hello world", propertyNameAndValues["Sub.SubString"]);
        }
        
        [TestMethod]
        public void Multiple_Formats2_Test()
        {
            TestObj x1 = InitializeMocks();
            var x = new StringFormatterCollection();
            x.Add(new ExpressionFormatter<TestObj>(o => o.MyDateTime.ToShortTimeString()));
            x.Add(new ExpressionFormatter<DateTime>(o => o.ToShortDateString()));

            var propertyNameAndValues = new PropertyMapper(x).GetMappings(x1);
            Assert.AreEqual(new DateTime(2011, 1, 1, 12, 1, 1).ToShortTimeString(), propertyNameAndValues["MyDateTime"]);
            Assert.AreEqual(new DateTime(2011, 2, 2, 12, 2, 2).ToShortDateString(), propertyNameAndValues["Sub.SubDateTime"]);
        }

        [TestMethod]
        public void Enum_Format_Test()
        {
            TestObj x1 = InitializeMocks();
            var x = new StringFormatterCollection();
            x.Add(new ExpressionFormatter<TestObj>(o => o.MyEnum.ToString() + " hello"));

            var propertyNameAndValues = new PropertyMapper(x).GetMappings(x1);
            Assert.AreEqual("e1 hello", propertyNameAndValues["MyEnum.e1"]);
        }
        
        [TestMethod]
        public void Enum_And_Bool_Format_Test()
        {
            TestObj x1 = InitializeMocks();
            var x = new StringFormatterCollection();
            x.Add(new ExpressionFormatter<bool>(o => o ? "Yes" : "No"));

            var propertyNameAndValues = new PropertyMapper(x).GetMappings(x1);
            Assert.AreEqual("e1", propertyNameAndValues["MyEnum"]);
            Assert.AreEqual("Yes", propertyNameAndValues["MyEnum.e1"]);
            Assert.AreEqual("No", propertyNameAndValues["MyEnum.e2"]);
        }

        [TestMethod]
        public void Bool_Format_Test()
        {
            TestObj x1 = InitializeMocks();
            var x = new StringFormatterCollection();
            x.Add(new ExpressionFormatter<bool>(o => o ? "Yes" : "No"));

            var propertyNameAndValues = new PropertyMapper(x).GetMappings(x1);
            Assert.AreEqual("Yes", propertyNameAndValues["MyBool"]);
        }

        [TestMethod]
        public void ComplexList_Format_Test()
        {
            TestObj x1 = InitializeMocks();
            var x = new StringFormatterCollection();
            x.Add(new ExpressionFormatter<string>(o => o.ToUpper()));

            var propertyNameAndValues = new PropertyMapper(x).GetMappings(x1);

            Assert.AreEqual("LIST STRING 1", propertyNameAndValues["MyComplexList[0].SubString"]);
        }
    }
}