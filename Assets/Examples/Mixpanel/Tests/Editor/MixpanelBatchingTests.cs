using NUnit.Framework;
using Assert = UnityEngine.Assertions.Assert;

namespace mixpanel
{
    [TestFixture]
    public class MixpanelTests
    {
        [Test]
        public void Identify_Test()
        {
            Mixpanel.Identify("Hello World");
            Assert.IsTrue(Mixpanel.DistinctId == "Hello World");
        }
    }
}
