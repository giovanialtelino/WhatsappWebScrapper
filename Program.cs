using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using AngleSharp.

namespace WhatsappWebScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Wait for the browser!");

            using (IWebDriver driver = new FirefoxDriver())
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(10));

                driver.Navigate().GoToUrl("https://web.whatsapp.com/");

                Console.Write("Login and press enter");
                Console.ReadKey();

                var listNames = driver.FindElements(By.ClassName("_210SC"));

                System.Console.WriteLine(listNames.Count);

                foreach (var item in listNames)
                {
                    var name = item.FindElement(By.ClassName("_357i8"));

                    item.Click();

                    var messages = item.FindElements(By.ClassName("_2hqOq"));
                    var toScroll = item.FindElement(By.ClassName("_2-aNW"));

                    var actions = new Actions(driver);

                    for (int i = 0; i < 50; i++)
                    {
                        actions.MoveByOffset(0,0);
                    }
                    actions.MoveByOffset(0,0);

                    foreach (var message in messages)
                    {
                        var text = message.FindElement(By.ClassName("copyable-text"));
                        var data = text.GetAttribute("data-pre-plain-text");

                        System.Console.WriteLine(text.Text);
                        System.Console.WriteLine(data);
                    }
                }
            }
        }
    }
}
