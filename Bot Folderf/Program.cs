
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;


namespace WebDriver_CHashtag_Example
{

    public class Program
    {
        static private IDictionary<int, string> Links = new Dictionary<int,string>();
        static private IDictionary<int, string> Episodes = new Dictionary<int,string>();
        public static string latestMessage = " ";
        public static string currentMessage = " ";
        public static int currentEpisode = 1;
        public static Boolean episodeFinised = false;
        public static Boolean seriesFinished = false;
        public static IReadOnlyList<IWebElement> listMessages;
        public static IWebElement sendMessage;
        static private ChromeDriver syncsiteDriver = new ChromeDriver();


        static void Main(string[] args) 
        {
            SetupRoom();
            //start listening for chat commands
            IWebElement chat = syncsiteDriver.FindElement(By.Id("chat"));
            IWebElement sendMessage = syncsiteDriver.FindElement(By.Id("message"));

            sendMessage.SendKeys("Starting");
            sendMessage.SendKeys(Keys.Enter);
            syncsiteDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            IReadOnlyList<IWebElement> listMessages = syncsiteDriver.FindElements(By.CssSelector("div.well.well-sm.message-well"));
            IWebElement changeButton = syncsiteDriver.FindElement(By.Id("changeButton"));
            IWebElement directLinkInput = syncsiteDriver.FindElement(By.Id("inputVideoId"));
            string temp = "";

            while(latestMessage != "quit") 
            {
                currentMessage = latestMessage;
                switch (currentMessage.Trim()) 
                {
                    case "search":
                        sendMessage.SendKeys("Enter The Anime:");
                        sendMessage.SendKeys(Keys.Enter);
                        System.Threading.Thread.Sleep(1000);
                        listMessages = syncsiteDriver.FindElements(By.CssSelector("div.well.well-sm.message-well"));
                        temp= listMessages[listMessages.Count -1].GetAttribute("innerText");
                        latestMessage = temp.Substring(temp.LastIndexOf(":")+1);
                        latestMessage = latestMessage.Substring(latestMessage.LastIndexOf(Environment.NewLine)+1).Trim();
                        currentMessage = latestMessage;

                        while(latestMessage.Equals(currentMessage)) 
                        {
                            System.Threading.Thread.Sleep(300);
                            listMessages = syncsiteDriver.FindElements(By.CssSelector("div.well.well-sm.message-well"));
                            temp= listMessages[listMessages.Count -1].GetAttribute("innerText");
                            latestMessage = temp.Substring(temp.LastIndexOf(":")+1);
                            latestMessage = latestMessage.Substring(latestMessage.LastIndexOf(Environment.NewLine)+1).Trim();
                        
                        }
                        listMessages = syncsiteDriver.FindElements(By.CssSelector("div.well.well-sm.message-well"));
                        temp= listMessages[listMessages.Count -1].GetAttribute("innerText");
                        latestMessage = temp.Substring(temp.LastIndexOf(":")+1);
                        latestMessage = latestMessage.Substring(latestMessage.LastIndexOf(Environment.NewLine)+1).Trim();
                        //BrowseArrayAnime(latestMessage , syncsiteDriver);
                        BrowseArrayAnime(latestMessage,syncsiteDriver);
                        directLinkInput.SendKeys(Episodes[currentEpisode]);
                        syncsiteDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(150);
                        changeButton.Click();
                        directLinkInput.SendKeys("");
                        break;
                    case "next":
                        currentEpisode+=1;
                        directLinkInput = syncsiteDriver.FindElement(By.Id("inputVideoId"));
                        directLinkInput.Clear();
                        directLinkInput.SendKeys(Episodes[currentEpisode]);
                        syncsiteDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(150);
                        changeButton.Click();                        
                        sendMessage.SendKeys($"Starting Episode {currentEpisode}");
                        sendMessage.SendKeys(Keys.Enter);

                    break;

                    default:
                        Console.WriteLine(latestMessage);
                        break;

                }
                System.Threading.Thread.Sleep(1000);
                listMessages = syncsiteDriver.FindElements(By.CssSelector("div.well.well-sm.message-well"));
                temp= listMessages[listMessages.Count -1].GetAttribute("innerText");
                latestMessage = temp.Substring(temp.LastIndexOf(":")+1);
                latestMessage = latestMessage.Substring(latestMessage.LastIndexOf(Environment.NewLine)+1).Trim();
            }
            sendMessage.SendKeys("Exitting...");
            sendMessage.SendKeys(Keys.Enter);
            syncsiteDriver.Quit();
        }
    
        static void SetupRoom()
        {
            // navigate to vynschorize and create a room
            syncsiteDriver.Navigate().GoToUrl("https://vynchronize.herokuapp.com/");
            syncsiteDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            IReadOnlyList<IWebElement> Inputs = syncsiteDriver.FindElements(By.TagName("input"));
            Inputs[0].SendKeys("Bot Host");
            Inputs[1].SendKeys("AnimePogErinaBest");
            Inputs[2].Click();
            // swap the player to mp4 mode
            syncsiteDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            IReadOnlyList<IWebElement> dropdown = syncsiteDriver.FindElements(By.CssSelector("button.btn.btn-info.dropdown-toggle"));
            dropdown[0].Click();
            syncsiteDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(25);
            IReadOnlyList<IWebElement> dropdownitems = syncsiteDriver.FindElements(By.ClassName("dropdown-item"));
            dropdownitems[3].Click();
        }
        //array anime is broken
        static void BrowseArrayAnime(string animename, ChromeDriver Driver2) 
        {
            // Generate Chrome Window with AdBlock
            // ChromeOptions options = new ChromeOptions();
            // options.AddArgument("-load-extension=C:\\Users\\Nicholas Choi\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Extensions\\cjpalhdlnbpafiamejdnhcphjbkeiagm\\1.34.0_2");
            ChromeDriver driver = new ChromeDriver();
            IWebElement sendMessage = Driver2.FindElement(By.Id("message"));
            listMessages = Driver2.FindElements(By.CssSelector("div.well.well-sm.message-well"));
            string baseURL = "https://www.arrayanime.com";
            driver.Navigate().GoToUrl(baseURL + "/key/" + animename + "/1");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            IReadOnlyList<IWebElement> ListAnimes = driver.FindElements(By.ClassName("ani-info"));
            System.Threading.Thread.Sleep(150);
            int count = 1;
            // go through Array Anime
            foreach (IWebElement names in ListAnimes) {
                    string name = names.FindElement(By.TagName("h3")).GetAttribute("innerText");
                    string url = baseURL + "/ani/" + name.Replace(" ", "-");
                    sendMessage.SendKeys(name);
                    sendMessage.SendKeys(Keys.Enter);
                    Console.WriteLine(count+ " : " + name + ' ' + url);
                    Links.Add(count,url);
                    count += 1;

            }
            System.Threading.Thread.Sleep(150);
            string temp = "";
            listMessages = Driver2.FindElements(By.CssSelector("div.well.well-sm.message-well"));
            temp= listMessages[listMessages.Count -1].GetAttribute("innerText");
            latestMessage = temp.Substring(temp.LastIndexOf(":")+1);
            latestMessage = latestMessage.Substring(latestMessage.LastIndexOf(Environment.NewLine)+1).Trim();
            currentMessage = latestMessage;
            int intRead;
            while(true) 
            {   
                try{
                    intRead = Convert.ToInt32(latestMessage);
                    break;
                }
                catch (Exception e) {
                listMessages = Driver2.FindElements(By.CssSelector("div.well.well-sm.message-well"));
                temp= listMessages[listMessages.Count -1].GetAttribute("innerText");
                latestMessage = temp.Substring(temp.LastIndexOf(":")+1);
                latestMessage = latestMessage.Substring(latestMessage.LastIndexOf(Environment.NewLine)+1).Trim();
                }

            }

            // pick specific anime/movie
            intRead = Convert.ToInt32(latestMessage);
            Console.WriteLine(latestMessage);
            driver.Quit();
            driver = new ChromeDriver();
            if (Links.TryGetValue(intRead, out string temp2)) {
                sendMessage.SendKeys("Found");
                sendMessage.SendKeys(Keys.Enter);
                driver.Navigate().GoToUrl(temp2);
                Episodes.Clear();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
                IReadOnlyCollection<IWebElement> episodeList = driver.FindElements(By.ClassName("sc-hBEYos"));
                Dictionary<int, string> urllist = new Dictionary<int, string>();
                foreach (IWebElement episode in episodeList) {
                    Console.WriteLine("this is for loop");
                    string epiUrl = episode.GetAttribute("href");
                    Episodes.Add(int.Parse(episode.GetAttribute("innerText")), epiUrl);
                }
                // for each episode gointo the page and click switch then grab url

                foreach (KeyValuePair<int, string> item in Episodes) 
                {   
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(600);
                    driver.Navigate().GoToUrl(item.Value);
                    System.Threading.Thread.Sleep(1000);
                    IReadOnlyList<IWebElement> toggle = driver.FindElements(By.ClassName("sc-bBXqnf"));
                    toggle[0].Click();
                    System.Threading.Thread.Sleep(300);
                    IReadOnlyList<IWebElement> mp4 = driver.FindElements(By.TagName("source"));
                    Episodes[item.Key] = mp4[0].GetAttribute("src");
                    System.Threading.Thread.Sleep(150);

                }
                }
            else {
                Console.WriteLine("Broken");
                }
            driver.Quit();
            



        }
        
        static void Browse4anime(string animename, ChromeDriver Driver2) 
                {
            //Generate Chrome Window with AdBlock
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("-load-extension=C:\\Users\\Nicholas Choi\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Extensions\\cjpalhdlnbpafiamejdnhcphjbkeiagm\\1.34.0_2");
            ChromeDriver driver = new ChromeDriver(options);
            IWebElement sendMessage = Driver2.FindElement(By.Id("message"));
            listMessages = Driver2.FindElements(By.CssSelector("div.well.well-sm.message-well"));
            string baseURL = "https://www.4anime.to";
            driver.Navigate().GoToUrl(baseURL + "/anime/" + animename.Replace(" ", "-") );
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

            sendMessage.SendKeys("Found");
            sendMessage.SendKeys(Keys.Enter);
            Episodes.Clear();
            IWebElement episodeList = driver.FindElement(By.ClassName("episodes"));
            IReadOnlyCollection<IWebElement> episodeList2 = episodeList.FindElements(By.TagName("a"));
            Console.WriteLine(episodeList2.Count);
            Dictionary<int, string> urllist = new Dictionary<int, string>();
            foreach (IWebElement episode in episodeList2) {
                   string epiUrl = episode.GetAttribute("href");
                   Episodes.Add(int.Parse(episode.GetAttribute("innerText")), epiUrl);
            }
            // // for each episode gointo the page and click switch then grab url

            foreach (KeyValuePair<int, string> item in Episodes) 
            {
                driver.Navigate().GoToUrl(item.Value);

                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                IReadOnlyList<IWebElement> mp4 = driver.FindElements(By.TagName("source"));
                Episodes[item.Key] = mp4[0].GetAttribute("src");
                Console.WriteLine(mp4[0].GetAttribute("src"));

            }

            driver.Quit();
            



        }
        static void BrowseTwistMoe(string animename, ChromeDriver Driver2) 
        {
            //Generate Chrome Window with AdBlock
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("-load-extension=C:\\Users\\Nicholas Choi\\AppData\\Local\\Google\\Chrome\\User Data\\Default\\Extensions\\cjpalhdlnbpafiamejdnhcphjbkeiagm\\1.34.0_2");
            ChromeDriver driver = new ChromeDriver();
            IWebElement sendMessage = Driver2.FindElement(By.Id("message"));
            listMessages = Driver2.FindElements(By.CssSelector("div.well.well-sm.message-well"));
            string baseURL = "https://www.twist.moe";
            driver.Navigate().GoToUrl(baseURL + "/key/" + animename + "/1");
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
            IReadOnlyList<IWebElement> ListAnimes = driver.FindElements(By.ClassName("ani-info"));
            System.Threading.Thread.Sleep(150);
            int count = 1;
            


        }
    }
}



