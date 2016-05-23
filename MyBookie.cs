using System;
using System.Text;
using System.Collections.Generic;
using HtmlAgilityPack;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.Extensions;
using WatiN.Core;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace SportsBetting
{
    public class MyBookie : WebsiteScraper
    {
        string fileName = "MyBookie.ag";
        List<Game> finishedGames = new List<Game>();
        bool queued;

        public MyBookie()
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
            try
            {
                document2.Load(@Helper.logPath + fileName + sport + "Pulled.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("couldnt load in pulled data to scrape " + getName());
            }

            HtmlNodeCollection nodes = document2.DocumentNode.SelectNodes("//*[contains(concat(\" \", normalize-space(@id), \" \"), \" span-lines \")]");
            if (nodes == null)
            {
                Console.WriteLine("Nothing Found...");
            }
            else {

                foreach (HtmlNode item in nodes)
                {
                    //int numberOfGames = item.SelectNodes("./h3").Count;
                    
                    HtmlNodeCollection allRows = item.SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" col-xs-12 row-offset-0 \")]");

                    if(allRows == null)
                    {
                        Console.WriteLine("Error: Couldn't find any rows! MyBookie");
                        break;
                    }
                    else
                    {
                        int x = 0;
                        while(x < allRows.Count)
                        {
                            Game tempGame = new Game();
                            Team homeTempTeam = new Team();
                            Team awayTempTeam = new Team();

                            Regex rgx = new Regex("[^a-zA-Z ]");//get ready to remove everything but letters

                            HtmlNodeCollection homeName = allRows[x].SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" col-lg-4 col-md-4 col-sm-4 col-xs-3 row-offset-0 border-left-lines \")]");
                            HtmlNodeCollection awayName = allRows[x+1].SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" col-lg-4 col-md-4 col-sm-4 col-xs-3 row-offset-0 border-left-lines \")]");

                            string homeTeamName;
                            string awayTeamName;

                            if (homeName == null || awayName == null || homeName.Count > 1 || awayName.Count > 1)
                            {
                                Console.WriteLine("Error: too many or too few game names MyBookie");
                                break;
                            }
                            else
                            {
                                homeTeamName = rgx.Replace(homeName[0].InnerText, "");//get rid of letters
                                awayTeamName = rgx.Replace(awayName[0].InnerText, "");//get rid of letters
                                homeTeamName = homeTeamName.Replace("\t", "").Replace("\n", "").Replace(" ", "-").Replace("\r", "").Replace(".", "").ToUpper();
                                awayTeamName = awayTeamName.Replace("\t", "").Replace("\n", "").Replace(" ", "-").Replace("\r", "").Replace(".", "").ToUpper();
                            }

                            homeTempTeam.name = Helper.replaceTeamName(homeTeamName, fileName);
                            awayTempTeam.name = Helper.replaceTeamName(awayTeamName, fileName);


                            HtmlNodeCollection homeLine = allRows[x].SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" regular-line \")]");
                            HtmlNodeCollection awayLine = allRows[x+1].SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" regular-line \")]");

                            string homeMoneyLine;
                            string awayMoneyLine;

                            if(homeLine == null || awayName == null || homeLine.Count > 3 || awayLine.Count > 3)
                            {
                                Console.WriteLine("Error: too many or too few lines in MyBookie");
                               
                                break;
                            }
                            else
                            {
                                homeMoneyLine =  Helper.parseMoneyLine(homeLine[0].InnerText,awayLine[0].InnerText);
                                awayMoneyLine = Helper.parseMoneyLine(awayLine[0].InnerText, homeLine[0].InnerText);
                            }

            

                            try
                            {
                                homeTempTeam.moneyLine = Int32.Parse(homeMoneyLine);
                                awayTempTeam.moneyLine = Int32.Parse(awayMoneyLine);
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine("Error in parsing " + fileName + " " + e);
                            }
                            

                            tempGame.homeTeam = homeTempTeam;
                            tempGame.awayTeam = awayTempTeam;
                            tempGame.scraper = fileName;

                            finishedGames.Add(tempGame);

                            x += 2;
                        }
                    }
                }

                dumpScrapedData();
            }


        }//end scrape data

        public void startPull(string sportName)
        {
            MainForm.logConsole("--Starting pull of MyBookie--");

            string url = "";
    
            if (sportName == "NBA")
            {
                url = "http://mybookie.ag/sportsbook/nba-betting-lines/";
            }
            else if (sportName == "NCAABasketball")
            {
                url = "http://mybookie.ag/sportsbook/ncaab-betting-lines/";
            }
            else if(sportName == "MLB")
            {
                url = "http://mybookie.ag/sportsbook/mlb-betting-lines/";
            }

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (obj, eb) => pullData(url, sportName);
            bw.RunWorkerCompleted += (obj, eb) => MainClass.GlobalArbitrage.finishedAPull(this);
            bw.RunWorkerAsync();
        }

        void pullData(string inputURL, string sportName)
        {
            Console.WriteLine("Starting Pull of " + inputURL);
            PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService("E:\\SportsBetting\\packages\\PhantomJS.2.1.1\\tools\\phantomjs");
            service.IgnoreSslErrors = true;
            service.SslProtocol = "any";
            PhantomJSDriver driver = new PhantomJSDriver(service);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
           
            string source = "";
            try
            {

                driver.Navigate().GoToUrl(inputURL);
                driver.FindElementByClassName("timezone");

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

