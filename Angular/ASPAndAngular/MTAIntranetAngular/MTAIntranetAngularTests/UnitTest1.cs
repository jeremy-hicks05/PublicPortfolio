using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using MTAIntranetAngular.API.Data.Models;
using OpenQA.Selenium.Support.UI;

namespace MTAIntranetAngularTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SQLReadTest()
        {
            Org1Context kaceContext = new Org1Context();
            var ticket = kaceContext.HdTickets.FirstOrDefault();
            Assert.NotNull(ticket); // Ensure at least one ticket exists
            Assert.NotNull(ticket.Title);
            //Assert.IsTrue(ticket.Title.Contains("Website")); // Validate ticket content

        }

        [Test]
        public void OpenWebsite()
        {
            //Assert.Pass();
            IWebDriver driver = new ChromeDriver();

            // 2. Navigate to the URL
            //driver
            driver.Navigate().GoToUrl("https://mtadev.mta-flint.net");
            driver.Manage().Window.Maximize();

            IWebElement webElement = driver.FindElement(
                By.XPath("/html/body/app-root/div/app-home/mat-grid-list/div/mat-grid-tile[1]/div/button/span[2]/p")
                );
            webElement.Click();
            driver.Close();
        }

        [Test]
        public void TestDropdown()
        {
            //Assert.Pass();
            IWebDriver driver = new ChromeDriver();

            // 2. Navigate to the URL
            //driver
            driver.Navigate().GoToUrl("https://mtadev.mta-flint.net");
            driver.Manage().Window.Maximize();

            IWebElement webElement = driver.FindElement(
                By.XPath("//p[text() = 'Enter Ticket']")
                );
            webElement.Click();

            //IWebElement dropdown = driver.FindElement(By.Id("hdQueueId"));
            //SelectElement selectElement = new(dropdown);
            //selectElement.SelectByIndex(0);

            // Locate the mat-select dropdown using its ID
            var matSelect = driver.FindElement(By.Id("hdQueueId"));

            // Click to open the dropdown
            matSelect.Click();

            // Wait for the dropdown options to be visible
            //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            //wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector("mat-option")));

            // Select the desired option (e.g., based on text or value)
            //var option = driver.FindElement(By.Id("mat-option-0"));
            var option = driver
                .FindElement(By.XPath("/html/body/div[2]/div[2]/div/div/mat-option[1]"));
            option.Click();

            // Optional: Assert the selected value or state
            //Assert.AreEqual("Desired Option Text", matSelect.Text);
            driver.Close();
        }

        [Test]
        public void CustomTest()
        {
            //Assert.Pass();
            IWebDriver driver = new ChromeDriver();

            // 2. Navigate to the URL
            //driver
            driver.Navigate().GoToUrl("https://mtadev.mta-flint.net");
            driver.Manage().Window.Maximize();

            IWebElement webElement = driver.FindElement(
                By.XPath("//p[text() = 'Enter Ticket']")
                );
            webElement.Click();

            // Locate the mat-select dropdown using its ID
            var matSelect = driver.FindElement(By.Id("hdQueueId"));

            // Click to open the dropdown
            matSelect.Click();

            // Select the desired option (e.g., based on text or value)
            //var option = driver.FindElement(By.Id("mat-option-0"));
            //option.Click();
            //SeleniumCustomMethods.Click(driver, By.Id("mat-option-0"));
            SeleniumCustomMethods.Click(driver, By.XPath("/html/body/div[2]/div[2]/div/div/mat-option[1]"));
            driver.Close();
        }
    }
}