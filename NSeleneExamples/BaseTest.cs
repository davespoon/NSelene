using NUnit.Framework;
using static NSelene.Selene;
using OpenQA.Selenium.Chrome;
using NSelene;
using OpenQA.Selenium.Firefox;

namespace NSeleneExamples
{
    [TestFixture()]
    public class BaseTest
    {
        [SetUp]
        public void SetupTest()
        {
            SetWebDriver(new FirefoxDriver());
            Configuration.Timeout = 6;
        }

        [TearDown]
        public void TeardownTest()
        {
            GetWebDriver().Quit();
        }
    }
}