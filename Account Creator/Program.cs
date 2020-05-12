using System;
using System.Collections.Generic;
using System.Globalization;
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
            Console.WriteLine("Do you want to open it in silent mode - write 'y' or 'n'");
            var answer = Console.ReadLine();
            Console.WriteLine("Write number of users");
            var numOfUsers = uint.Parse(Console.ReadLine());
            Dictionary<string, string> credentials = CreateNumOfUsers(numOfUsers, answer);

            using StreamWriter writer = new StreamWriter(Environment.CurrentDirectory + "\\credentials.txt", false);
            foreach (var credential in credentials)
            {
                writer.WriteLineAsync(@$"{credential.Key} : {credential.Value}");
            }
        }

        private static Dictionary<string, string> CreateNumOfUsers(uint numberOfUsers, string answer)
        {
            var credentials = new Dictionary<string, string>();
            while (credentials.Count != numberOfUsers)
            {
                var (userName, userPassword) = CreateOneUser(answer);
                if (userPassword == "" || userName == "") continue;

                credentials.Add(userName, userPassword);
                PercentOfWork(numberOfUsers, credentials.Count);
            }

            return credentials;
        }

        private static (string, string) CreateOneUser(string answer)
        {
            var options = new ChromeOptions();
            if (answer == "y")
            {
                options.AddArgument("--headless");
            }
            options.AddArgument("--log-level=3");
            options.SetLoggingPreference(LogType.Server, LogLevel.All);
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

                Thread.Sleep(10000);
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                var asd = _driver.FindElement(By.Id("menuAccountUsername")).Text;

                if (_driver.FindElement(By.Id("menuAccountUsername")).Text != userName) return ("", "");



                _driver.Close();

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

        private static void PercentOfWork(uint needed, int ready)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Clear();
            Console.WriteLine($"Needed: {needed} - Ready:{ready}");
            Console.WriteLine($"Work is DONE");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}