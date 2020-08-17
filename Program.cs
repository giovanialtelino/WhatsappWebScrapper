using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using System.Text.Json;
using System.IO;

namespace WhatsappWebScrapper
{
    class Program
    {
        static List<Chat> ExtractCurrentNames(IWebDriver d)
        {
            var tempList = new List<Chat>();
            var step1 = d.FindElements(By.CssSelector("div[class='_3CneP']"));

            foreach (var item in step1)
            {
                var step2 = item.FindElement(By.CssSelector("span[class='_3ko75 _5h6Y_ _3Whw5']"));
                var tempChat = new Chat(step2.Text);
                step2.Click();

                Thread.Sleep(TimeSpan.FromSeconds(5));
                var chat = ExtractCurrentChat(d);
                tempChat.AddMessages(chat);

                tempList.Add(tempChat);
            }
            return tempList;
        }

        static List<ChatMessages> ExtractCurrentChat(IWebDriver d)
        {
            var tempList = new List<ChatMessages>();
            var step1 = d.FindElements(By.CssSelector("div._2hqOq.message-in.focusable-list-item"));
            foreach (var item in step1)
            {
                if (item.FindElements(By.CssSelector("div[class*='copyable-text']")).Count > 0)
                {
                    var step2 = item.FindElement(By.CssSelector("div[class*='copyable-text']"));
                    var nameAndTime = step2.GetAttribute("data-pre-plain-text").ToString().Split("]");
                    if (item.FindElements(By.CssSelector("div[class*='eRacY']")).Count > 0)
                    {
                        if (item.FindElement(By.CssSelector("div[class='eRacY']")).FindElements(By.CssSelector("span[class*='_3Whw5']")).Count > 0)
                        {
                            var message = step2.FindElement(By.CssSelector("div[class='eRacY']")).FindElement(By.CssSelector("span[class*='_3Whw5']")).Text;
                            var name = nameAndTime[0];
                            var time = nameAndTime[1];
                            System.Console.WriteLine("--------");
                            System.Console.WriteLine(nameAndTime[0]);
                            System.Console.WriteLine(nameAndTime[1]);
                            System.Console.WriteLine(message);
                            System.Console.WriteLine("--------");
                            tempList.Add(new ChatMessages(name, message, time));
                        }
                    }
                }
            }
            return tempList;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Wait for the browser!");

            using (IWebDriver driver = new FirefoxDriver())
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

                driver.Navigate().GoToUrl("https://web.whatsapp.com/");

                Console.Write("Login and press enter");
                Console.ReadKey();

                var namesList = new List<Chat>();
                namesList.AddRange(ExtractCurrentNames(driver));


                var newElements = true;
                while (newElements)
                {
                    var elementToScrollDown = driver.FindElement(By.ClassName("_3R02z"));
                    newElements = false;
                    elementToScrollDown.SendKeys(Keys.PageDown);
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    var tempListNamesString = ExtractCurrentNames(driver);

                    foreach (var item in tempListNamesString)
                    {
                        if (namesList.Contains(item) == false)
                        {
                            newElements = true;
                            namesList.Add(item);
                        }
                    }
                }

                var resultFile = String.Concat(Directory.GetCurrentDirectory(), "/whatsapp.json");

                using (var outputFile = new StreamWriter(resultFile))
                {
                    outputFile.Write("[");

                    for (int i = 0; i < namesList.Count - 2; i++)
                    {
                        outputFile.Write(JsonSerializer.Serialize(namesList[i]));
                        outputFile.Write(", ");
                    }

                    var finalChat = namesList[namesList.Count - 1];
                    outputFile.Write(JsonSerializer.Serialize(finalChat));

                    outputFile.Write("]");

                }

            }
        }
    }
}
