using NUnit.Framework;
using System;
using Moq;

namespace ConsoleApp1.Tests
{
    public class Test1
    {
        public Test1(OtherClass item)
        {

        }
    }

    public abstract class OtherClass
    {
        public int DoSth()
        {
            return 5;
        }
    }

    [TestFixture]
    public class TestClassk
    {
        [Test]
        public void METHOD()
        {
            var item = new Mock<OtherClass>();
            
            var t1 = new{on} Test1({on});
        }
    }
}
