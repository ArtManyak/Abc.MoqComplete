// ${COMPLETE_ITEM:new Mock<ITestInterface>().Object}
using Moq;
using NUnit.Framework;

namespace ConsoleApp1.Tests
{
    public interface ITestInterface
    {
        void BuildSomething(int i, string toto, bool ok);
    }
	
	public class Temp
    {
        private readonly ITestInterface myTestInterface;

        public Temp(ITestInterface testInterface)
        {
            myTestInterface = testInterface;
        }
    }

    [TestFixture]
    public class Test1
    {
        [Test]
        public void METHOD()
        {
            var obj = new Temp(mock{caret});
        }
    }
}