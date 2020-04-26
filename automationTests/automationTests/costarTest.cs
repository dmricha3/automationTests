using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using System.Diagnostics;
using System.Threading;

namespace automationTests
{
    public class costarTest
    {

        private IWebDriver driver;

        public void WaitUntilElementPresent(By elementLocator, int timeout = 10)
        {
            //Declaring the variable implicitTimeOut and setting it equal to
            //the current driver impicit wait.
            var implicitTimeOut = driver.Manage().Timeouts().ImplicitWait;
            //This setting the implicit wait equal to 1 second
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(1);
            //declaring the variable remainingTime and setting it equal to the passed in parameter
            var remainingTime = timeout;
            System.Diagnostics.Stopwatch sp = new Stopwatch();
            sp.Restart();
            var counter = 0;
            while (remainingTime > 0)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"We hit the try! Remaining Time: {remainingTime}");
                    driver.FindElement(elementLocator);
                    driver.Manage().Timeouts().ImplicitWait = implicitTimeOut;
                    return;
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("We hit the catch!");
                }
                counter++;
                remainingTime--;
                //Thread.Sleep(1000);
            }

            throw new Exception($"Element not found: {elementLocator.ToString()} waited for {sp.ElapsedMilliseconds} ms checked {counter} times");
            //This will throw the error as well as the element it happened on.
        }
        [SetUp]
        public void Setup()
        {
            //var options = new ChromeOptions();
            //options.AddArgument("no-sandbox");
            driver = new ChromeDriver("/Users/david.richards/projects/QuickAutomationRepo/QuickAutomationRepo/");
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = System.TimeSpan.FromSeconds(10);
        }
        [Test]
        //This test will be to ensure that the "Request a Demo" button is working properly.
        public void costar()
        {
            //This will open the site we are going to be testing
            driver.Url = "https://www.costar.com";

            //This will find the "Contact Us" section and click it and wait for the "Request a Demo
            //button to open in a new tab.
            driver.FindElement(By.XPath("(//*[@class='NavBarMenu_menu-labels'])[3]")).Click();
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            WaitUntilElementPresent(By.XPath("//*[@class='visible-sm visible-lg visible-xs demoSubmitBtn']"));

            //This will click the "Request a Demo" button and wait for it to load the new page and
            //wait for the inormation submit button to be present.
            driver.FindElement(By.XPath("//*[@value='Request a Demo']")).Click();
            WaitUntilElementPresent(By.XPath("//*[@class='demoSubmitBtn']"));
        }
        [OneTimeTearDown]
        public void End()
        {
            driver.Close();
            driver.Dispose();
        }
    }
}
