using System;
using System.Text;
using System.Collections.Generic;
using HtmlAgilityPack;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.Extensions;
using WatiN.Core;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading;

namespace SportsBetting
{
    public class BetDSI : WebsiteScraper
    {
        string fileName = "BetDSI.eu";
        List<Game> finishedGames = new List<Game>();
        bool queued;

        string dropDownLink = "";
        string targetLink = "";

        public BetDSI()
        {
        }

        public List<Game> getGames()
        {
            return finishedGames;
        }

        public void scrapeData(string sport)
        {
            Console.WriteLine("--Starting Scrape of " + fileName);

            finishedGames.Clear();

            HtmlDocument document2 = new HtmlDocument();
            //document2.Load(@"c:\\Users\\Luke\\Desktop\\test.txt");  

            try
            {
                document2.Load(@Helper.logPath + fileName + sport + "Pulled.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("couldnt load in pulled data to scrape " + getName());
            }

            //HtmlNodeCollection externalLinePages = document2.DocumentNode.SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" externalLinesPage \")]");

            //HtmlNodeCollection topRow = externalLinePages[0].SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" topRow \")]");
            HtmlNodeCollection topRow = document2.DocumentNode.SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" topRow \")]");
            HtmlNodeCollection botRow = document2.DocumentNode.SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" botRow \")]");
            HtmlNodeCollection timeRow = document2.DocumentNode.SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" lineColA \")]/small");

            //Console.WriteLine(externalLinePages.Count);
            //Console.WriteLine(topRow.Count);

            //getting date of game
            HtmlNodeCollection testDate = document2.DocumentNode.SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" externalLinesPage \")]");
            

            if (topRow == null || botRow == null || timeRow == null || topRow.Count != botRow.Count)
            {
                Helper.writeError("one of the rows is null or they are mismatched lengths",fileName+sport);
            }
            else
            {
                   
                int x = 0;
                while(x < topRow.Count)
                {
                    Game game = new Game();
                    Team homeTeam = new Team();
                    Team awayTeam = new Team();

                    HtmlNode homeName = topRow[x].SelectSingleNode("./td[@class='lineName']/strong");
                    HtmlNode awayName = botRow[x].SelectSingleNode("./td[@class='lineName']/strong");

                    if (homeName == null || awayName == null)
                    {
                        Helper.writeError("Couldn't find home or away name", fileName + sport);
                        break;
                    }
                    if (timeRow[x] == null)
                    {
                        Helper.writeError("Couldn't find time stamp", fileName + sport);
                        
                    }


                    

                    string homeTeamName = homeName.InnerText.Replace("\t", "").Replace("\n", "").Replace(" ", "-").Replace("\r", "").Replace(".", "").ToString();
                    string awayTeamName = awayName.InnerText.Replace("\t", "").Replace("\n", "").Replace(" ", "-").Replace("\r", "").Replace(".", "").ToString();

                    homeTeamName = Helper.replaceTeamName(homeTeamName, fileName);
                    awayTeamName = Helper.replaceTeamName(awayTeamName, fileName);

                    HtmlNode topRowMline = topRow[x].SelectSingleNode("./td[@class='lineTotal']");
                    HtmlNode botRowMline = botRow[x].SelectSingleNode("./td[@class='lineTotal']");

                    if(topRowMline == null || botRowMline == null)
                    {
                        Helper.writeError("Couldn't find line Total", fileName + sport);
                        break;
                    }

                    HtmlNode topRowMoneyLine = topRowMline.SelectNodes("./span")[0];
                    HtmlNode botRowMoneyLine = botRowMline.SelectNodes("./span")[0];

                    if (topRowMoneyLine == null || botRowMoneyLine == null)
                    {
                        Helper.writeError("Couldn't find money line", fileName + sport);
                        break;
                    }
                    
                    string homeMoneyLine = topRowMoneyLine.InnerText.Replace("&nbsp;", "");
                    string awayMoneyLine = botRowMoneyLine.InnerText.Replace("&nbsp;", "");

                    homeMoneyLine = homeMoneyLine.Replace("\t", "").Replace("\n", "").Replace(" ", "").Replace("\r", "");
                    awayMoneyLine = awayMoneyLine.Replace("\t", "").Replace("\n", "").Replace(" ", "").Replace("\r", "");
                    homeMoneyLine = Helper.parseMoneyLine(homeMoneyLine, awayMoneyLine);
                    awayMoneyLine = Helper.parseMoneyLine(awayMoneyLine, homeMoneyLine);

                    homeTeam.name = homeTeamName;
                    awayTeam.name = awayTeamName;


                    try
                    {
                        homeTeam.moneyLine = Int32.Parse(homeMoneyLine);
                        awayTeam.moneyLine = Int32.Parse(awayMoneyLine);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error in parsing " + fileName + " " + e);
                    }


                    //getting time
                    string time = timeRow[x].InnerText.Replace("\t", "").Replace("\n", "").Replace(" ", "").Replace("\r", "").Replace(".", "").ToString();
                    DateTime timeStamp = DateTime.Parse(time).AddHours(3);//making it in edt not pac

                    


                    //getting date
                    int count = 0;
                    foreach (Game gm in finishedGames)
                    {
                        if (gm.homeTeam.name == homeTeam.name && gm.awayTeam.name == awayTeam.name && gm.time.Hour == timeStamp.Hour)
                        {
                            count++;
                        }
                    }

                    if (count < testDate.Count)
                    {
                        string date = testDate[count].SelectSingleNode(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" linesSubhead \")]").InnerText;
                        string[] dateSplit = date.Split('-');
                        try
                        {
                            timeStamp = DateTime.Parse(timeStamp.TimeOfDay + " " + dateSplit[1].Replace("&nbsp;", ""));
                        }
                        catch(Exception e)
                        {
                            Helper.writeError("BetDSI couldnt find date correctly", fileName + " date error: " + e);
                        }
                        
                        //Console.WriteLine(timeStamp.TimeOfDay);
                        
                        game.time = timeStamp;
                    }
                    else
                    {
                        Helper.writeError("bad count", fileName + "date error");
                    }






                    game.homeTeam = homeTeam;
                    game.awayTeam = awayTeam;
                    game.scraper = fileName;


                    finishedGames.Add(game);

                    x++;
                }

                dumpScrapedData();
            }


        }//end scrape data


        public void startPull(string sportName)
        {
            //Console.WriteLine("--Starting pull of " + fileName + "--");

            dropDownLink = "";
            targetLink = "";

            string url = "https://www.betdsi.eu/sportsbook-betting/";

            if (sportName == "NBA")
            {
                targetLink = "league3";
                dropDownLink = "#categoryBASKETBALL a";
            }
            else if (sportName == "NCAABasketball")
            {
                //url = "http://mybookie.ag/sportsbook/ncaab-betting-lines/";
            }
            else if(sportName == "MLB")
            {
                targetLink = "league5";
                dropDownLink = "#categoryBASEBALL a";
            }

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (obj, e) => pullData(url, sportName);
            bw.RunWorkerCompleted += (obj, e) => MainClass.GlobalArbitrage.finishedAPull(this);
            bw.RunWorkerAsync();

        }

        void pullData(string inputURL, string sportName)
        {
            Console.WriteLine("Starting Pull of " + inputURL);
            PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService("..\\..\\packages\\PhantomJS.2.1.1\\tools\\phantomjs");
            service.IgnoreSslErrors = true;
            service.SslProtocol = "any";
            service.LocalToRemoteUrlAccess = true;
            //service.AddArgument("--ignore-ssl-errors=true");
            // service.AddArgument("--ssl-protocol=tlsv1");
            //service.GhostDriverPath = "E:\\SportsBetting\\packages\\PhantomJS.2.1.1\\tools\\phantomjs";
            //PhantomJSDriver driver = new PhantomJSDriver("E:\\SportsBetting\\packages\\PhantomJS.2.1.1\\tools\\phantomjs");
            PhantomJSDriver driver = new PhantomJSDriver(service);
            driver.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
            //driver.Url = inputURL;

            string source = "";
            try
            {
                driver.Navigate().GoToUrl(inputURL);
            
                if (targetLink != "")
                {
                    //Console.WriteLine("here");

                    //var firstNext = driver.FindElementByCssSelector(dropDownLink);
                    //firstNext.Click();
                    //Thread.Sleep(5000);

                    var next = driver.FindElementById(targetLink);

                    next.Click();
                    
                    Thread.Sleep(10000);
                }

                source = driver.PageSource;
            }
            catch (Exception e)
            {
                Helper.writeError(e.ToString(), (fileName + sportName));
            }

            Helper.writePulledData(source, (fileName + sportName));
            driver.Quit();

            Console.WriteLine("Data pulled");
        }

        void dumpScrapedData()
        {
            string final = "";
            foreach (Game gm in finishedGames)
            {
                final += gm.dumpData();
            }

            Helper.writeScrapedData(final, fileName);
        }

        public string getName()
        {
            return fileName;
        }

        public bool isQueued()
        {
            return queued;
        }

        public void setQueued(bool input)
        {
            queued = input;
        }

        //used when we find an arbitrage, dont want to populate the sql table twice
        public void removeGame(int index)
        {
            finishedGames.RemoveAt(index);
        }

    }//end class
}//end file

