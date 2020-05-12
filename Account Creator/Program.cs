using System;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Account_Creator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            
                //CreateOneUser("139.180.190.74:8080");
                CreateOneUser("192.186.154.202");
            
        }

        private static (string, string) CreateOneUser(string proxy)
        {
            var options = new ChromeOptions();
            ////options.AddArgument("--headless");
            options.AddArgument("--log-level=OFF");
            //options.AddArgument($"--proxy-server={proxy}");
            options.AddArguments("--proxy-server=http://bhzkgypi-1:y7vb0a9oipg4@192.186.154.202:80");

            IWebDriver _driver = new ChromeDriver(options);

            _driver.SwitchTo();



            _driver.Url = "https://krunker.io";
            //_driver.Url = "https://2ip.io/";


            try
            {
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                var termsButtons = _driver.FindElements(By.ClassName("termsBtn"));
                termsButtons[1].Click();

                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                var logButtons = _driver.FindElement(By.CssSelector("div[class='menuItemIcon iconProfile']"));
                logButtons.Click();

                Thread.Sleep(5);
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);


                var (userName, userPassword) = UserCredentials();
                var accName = _driver.FindElement(By.Id("accName"));
                accName.SendKeys(userName);
                var accPass = _driver.FindElement(By.Id("accPass"));
                accPass.SendKeys(userPassword);

                var accountButtons = _driver.FindElements(By.ClassName("accountButton"));
                accountButtons[0].Click();

                Thread.Sleep(4000);

                if (_driver.FindElement(By.Id("accResp")).Text == "Register Error") return ("", "");

                _driver.Quit();

                return (userName, userPassword);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine("Element was not found");
                return ("", "");
            }
        }

        private static (string, string) UserCredentials()
        {
            const string chars = "qwertyuiopasdfghjklzxcvbnm1234567890";
            var length = 10;
            var random = new Random();
            var uname = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            var upassword =
                new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return (uname, upassword);
        }

       
    }
}