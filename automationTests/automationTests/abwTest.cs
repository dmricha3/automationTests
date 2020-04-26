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
    public class abwTest
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
        //This test will be to ensure that the beer to-go function is working properly.
        public void abw()
        {
            //This will open the site we are going to be testing
            driver.Url = "https://austinbeerworks.com/";

            //This will select "Yes" for being over 21
            driver.FindElement(By.XPath("(//*[@id='age-gate-yes'])")).Click();
            WaitUntilElementPresent(By.XPath("(//*[@href='https://squareup.com/market/austinbeerworks'])"));

            //This will will open the store in a new tab and wait for beer to-go options to be present.
            driver.FindElement(By.XPath("(//*[@href='https://squareup.com/market/austinbeerworks'])")).Click();
            driver.SwitchTo().Window(driver.WindowHandles.Last());
            WaitUntilElementPresent(By.XPath("(//*[@class='hover__background figure__hover hover__background--fade'])[6]"));

            
            //This will click a beer option and wait for it to load the desired page
            driver.FindElement(By.XPath("(//*[@class='hover__background figure__hover hover__background--fade'])[6]")).Click();
            WaitUntilElementPresent(By.XPath("//*[@class='w-button__label']"));
        }
        [OneTimeTearDown]
        public void End()
        {
            driver.Close();
            driver.Dispose();
        }
    }
}