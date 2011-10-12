using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net2PdfTests
{
    public class TestObj
    {
        public string MyString { get; set; }
        public int MyInt { get; set; }
        public SubOjb Sub { get; set; }
        public TestEnum MyEnum { get; set; }
        public List<string> MyList { get; set; }
        public List<SubOjb> MyComplexList { get; set; }
        public DateTime MyDateTime { get; set; }
        public bool MyBool { get; set; }

        public static TestObj InitializeMocks()
        {
            return new TestObj()
            {
                MyComplexList = new List<SubOjb>() { new SubOjb() { SubString = "List String 1" }, new SubOjb() { SubString = "List String 2" } },
                MyBool = true,
                MyInt = 10,
                MyString = "I'm A String",
                MyEnum = TestEnum.e1,
                Sub = new SubOjb() { SubString = "I'm a Sub String", Inner = new InnerObj() { InnerInt = 99 }, SubDateTime = new DateTime(2011, 2, 2, 12, 2, 2) },
                MyList = new List<string>() { "I'm", "a", "list" },
                MyDateTime = new DateTime(2011, 1, 1, 12, 1, 1)
            };
        }
    }

    public class SubOjb
    {
        public DateTime SubDateTime { get; set; }
        public string SubString { get; set; }
        public InnerObj Inner { get; set; }
    }

    public class InnerObj
    {
        public int InnerInt { get; set; }

        public override string ToString()
        {
            return "I'm the ToString of InnerObj" + InnerInt;
        }
    }

    public enum TestEnum
    {
        e1 = 1, e2 = 43, e3 = 4
    }


}
