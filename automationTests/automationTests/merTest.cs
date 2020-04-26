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
    public class merTest
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
        //This test will be to ensure that the curbside ordering function is working properly for Matt's El Rancho.
        public void mer()
        {
            //This will open the site we are going to be testing
            driver.Url = "https://mattselrancho.com/";

            //This will select the curbside pick-up uption.
            driver.FindElement(By.XPath("(//*[@class='menu-item menu-item-type-custom menu-item-object-custom menu-item-2249'])")).Click();
            WaitUntilElementPresent(By.XPath("(//*[@class='styles_categoryName__K9xVQ'])"));

            //This will will click an item and wait for a pop up to add it to the cart.
            driver.FindElement(By.XPath("(//*[@class='styles_name__2yjyg'])")).Click();
            WaitUntilElementPresent(By.XPath("(//*[@data-testid='AddToOrder'])"));


            //This will click "Add to Order" to the selected item and wait for the "Checkout" button to be available.
            driver.FindElement(By.XPath("(//*[@data-testid='AddToOrder'])")).Click();
            WaitUntilElementPresent(By.XPath("//*[@class='styles_btnWrapper__pqLZj']"));
        }
        [OneTimeTearDown]
        public void End()
        {
            driver.Close();
            driver.Dispose();
        }
    }
}