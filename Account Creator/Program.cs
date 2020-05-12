using System;
using System.Collections.Generic;
using System.IO;
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
            Console.WriteLine("Write number of users");

            Dictionary<string, string> credentials = CreateNumOfUsers(uint.Parse(Console.ReadLine()));

            using StreamWriter writer = new StreamWriter(Environment.CurrentDirectory + "\\credentials.txt", false);
            foreach (var credential in credentials)
            {
                writer.WriteLineAsync(@$"{credential.Key} : {credential.Value}");
            }
        }

        private static Dictionary<string, string> CreateNumOfUsers(uint numberOfUsers)
        {
            var credentials = new Dictionary<string, string>();
            while (credentials.Count != numberOfUsers)
            {
                var (userName, userPassword) = CreateOneUser();
                if (userPassword == "" || userName == "") continue;

                credentials.Add(userName, userPassword);
                PercentOfWork(credentials.Count, numberOfUsers);
            }

            return credentials;
        }

        private static (string, string) CreateOneUser()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");                //if it is ON - browser will not open and will work in silent mode
            options.AddArgument("--log-level=3");
            options.SetLoggingPreference("none", LogLevel.All);
            //options.AddArgument($"--proxy-server={proxy}");         //here you can put your proxy like --proxy-server=192.186.154.202:80

            IWebDriver _driver = new ChromeDriver(options);

            _driver.Url = "https://krunker.io";


            try
            {
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                Thread.Sleep(5);
                var termsButtons = _driver.FindElements(By.ClassName("termsBtn"));
                termsButtons[1].Click();

                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                Thread.Sleep(5);
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

                try
                {
                    _driver.FindElement(By.Id("menuWindow"));
                }
                catch
                {
                    Console.WriteLine("User was not created");
                    return ("", ""); //if menuWindow doesn't exist - user was not created
                }

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

        private static void PercentOfWork(int needed, uint ready)
        {
            Console.WriteLine($"Needed: {needed} - Ready:{ready}");
        }
    }
}