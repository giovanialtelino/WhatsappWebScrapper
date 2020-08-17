using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

namespace WhatsappWebScrapper
{
    class Program
    {
        static public string NameExtractor(IWebElement element)
        {
            ///html/body/div[1]/div/div/div[3]/div/div[2]/div[1]/div/div/div[15]/div/div/div[2]/div[1]/div[1]/span/span
            var name = element.FindElement(By.XPath("/html/body/div[1]/div/div/div[3]/div/div[2]/div[1]/div/div/div[15]/div/div/div[2]/div[1]/div[1]/span/span"));

            System.Console.WriteLine(name);

            return name.Text;
        }

        static public List<string> ListNameExtractor(ICollection<IWebElement> elements)
        {
            var tempList = new List<String>();
            foreach (var item in elements)
            {
                tempList.Add(NameExtractor(item));
            }
            return tempList;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Wait for the browser!");

            using (IWebDriver driver = new FirefoxDriver())
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

                driver.Navigate().GoToUrl("https://web.whatsapp.com/");
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(15);

                Console.Write("Login and press enter");
                Console.ReadKey();

                var listNamesString = new List<string>();

                var listNames = driver.FindElements(By.ClassName("_210SC"));
                listNamesString.AddRange(ListNameExtractor(listNames));
                var elementToScrollDown = driver.FindElement(By.ClassName("_3R02z"));


                var newElements = true;
                while (newElements)
                {
                    newElements = false;
                    elementToScrollDown.SendKeys(Keys.PageDown);
                    Thread.Sleep(TimeSpan.FromSeconds(10));
                    var tempLisNames = driver.FindElements(By.ClassName("_210SC"));
                    var tempListNamesString = ListNameExtractor(tempLisNames);

                    foreach (var item in tempListNamesString)
                    {
                        if (listNamesString.Contains(item) == false)
                        {
                            newElements = true;
                            listNamesString.Add(item);
                        }
                    }
                }

                System.Console.WriteLine("-----");
                System.Console.WriteLine(listNamesString.Count);
                foreach (var item in listNamesString)
                {
                    System.Console.WriteLine(item);
                }
                System.Console.WriteLine("------");

                // foreach (var item in listNames)
                // {
                //     var name = item.FindElement(By.ClassName("_357i8"));

                //     item.Click();

                //     var messages = item.FindElements(By.ClassName("_2hqOq"));
                //     var toScroll = item.FindElement(By.ClassName("_2-aNW"));

                //     var actions = new Actions(driver);

                //     for (int i = 0; i < 50; i++)
                //     {
                //         actions.MoveByOffset(0,0);
                //     }
                //     actions.MoveByOffset(0,0);

                //     foreach (var message in messages)
                //     {
                //         var text = message.FindElement(By.ClassName("copyable-text"));
                //         var data = text.GetAttribute("data-pre-plain-text");

                //         System.Console.WriteLine(text.Text);
                //         System.Console.WriteLine(data);
                //     }
                // }
            }
        }
    }
}
