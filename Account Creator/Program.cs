using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Account_Creator
{
    class Program
    {
        static void Main(string[] args)
        {
            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--headless");
            options.Proxy = new Proxy();
            IWebDriver _driver = new ChromeDriver(options);
            _driver.Url = "https://krunker.io";


            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);


            var termsButtons = _driver.FindElements(By.ClassName("termsBtn"));
            termsButtons[1].Click();

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);
            var logButtons = _driver.FindElement(By.CssSelector("div[class='menuItemIcon iconProfile']"));
            logButtons.Click();

            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);
            var accName = _driver.FindElement(By.Id("accName"));
            accName.SendKeys("asdf3t23fvw22");
            //accName.Submit();
            
            var accPass = _driver.FindElement(By.Id("accPass"));
            accPass.SendKeys("aszdfkmfgAWDd23r");
            //accPass.Submit();

            var accountButtons = _driver.FindElements(By.ClassName("accountButton"));
            accountButtons[0].Click();

            Console.WriteLine("Complete");


        }
    }
}
