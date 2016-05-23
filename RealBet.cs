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
    public class RealBet : WebsiteScraper
    {
        string fileName = "RealBet.eu";
        List<Game> finishedGames = new List<Game>();
        bool queued;

        public RealBet()
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
            catch(Exception e)
            {
                Console.WriteLine("couldnt load in pulled data to scrape " + getName());
            }
            

            HtmlNodeCollection nodes = document2.DocumentNode.SelectNodes("//*[contains(concat(\" \", normalize-space(@id), \" \"), \" events \")]");
            if (nodes == null)
            {
                Helper.writeError("Couldn't find any rows!", fileName + sport);
            }
            else {

                foreach (HtmlNode item in nodes)// loop through the hopefully only events tag we find
                {
                    int numberOfGames = item.SelectNodes("./h3").Count;

                    int x = 0;
                    while(x < numberOfGames)
                    {
                        Game tempGame = new Game();
                        Team homeTempTeam = new Team();
                        Team awayTempTeam = new Team();

                        //Team Names
                        HtmlNode title = item.SelectSingleNode("./h3[" + (x+1) + "]");

                        if(title == null)
                        {
                            Helper.writeError("Couldn't find title!", fileName + sport);
                            break;
                        }

                        Regex rgx = new Regex("[^a-zA-Z ]");//get ready to remove everything but letters
                       
                        HtmlNode strong = title.SelectSingleNode("./strong");//removing the strong tag which contains an O if there is one
                        if(strong == null)
                        {
                        }
                        else
                        {
                            strong = title.RemoveChild(strong); 
                        }
                        

                        string temp = rgx.Replace(title.InnerText, "");//get rid of letters
                        string[] teamNames = temp.Split(new string[] { " at " },StringSplitOptions.None);// split the names in 2

                        int y = 0;
                        while(y < teamNames.Length) { 
                            teamNames[y] = teamNames[y].Replace("\t", "").Replace("\n", "").Replace(" ", "-").Replace("\r", "").Replace(".", "").ToUpper();
                            teamNames[y] = Helper.replaceTeamName(teamNames[y], fileName);
                            y++;
                        }

                        homeTempTeam.name = teamNames[0];
                        awayTempTeam.name = teamNames[1];

                        //MoneyLine
                        HtmlNode moneyLineTable = item.SelectSingleNode("./table[" + (x + 1) + "]");
                        if(moneyLineTable == null)
                        {
                            Helper.writeError("Couldn't find moneyTable!", fileName + sport);
                            break;
                        }
                        HtmlNodeCollection homeTeamRow = moneyLineTable.SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" row0 \")]");
                        HtmlNodeCollection awayTeamRow = moneyLineTable.SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" row1 \")]");
                        if (homeTeamRow == null || awayTeamRow == null || homeTeamRow.Count > 1 || awayTeamRow.Count > 1)
                        {
                            Helper.writeError("Too many or too few rows!", fileName + sport);
                            break;
                        }
                        else {
                            HtmlNodeCollection homeTeamMoneyLine = homeTeamRow[0].SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" money \")]");
                            HtmlNodeCollection awayTeamMoneyLine = awayTeamRow[0].SelectNodes(".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" money \")]");
                            if (homeTeamMoneyLine == null || awayTeamMoneyLine == null || homeTeamMoneyLine.Count > 1 || awayTeamMoneyLine.Count > 1)
                            {
                                Helper.writeError("Too many or too few moneyLines!", fileName + sport);
                                break;
                            }
                            else
                            {
                                try
                                {
                                    homeTempTeam.moneyLine = Int32.Parse(Helper.parseMoneyLine(homeTeamMoneyLine[0].InnerText, awayTeamMoneyLine[0].InnerText));
                                    awayTempTeam.moneyLine = Int32.Parse(Helper.parseMoneyLine(awayTeamMoneyLine[0].InnerText, homeTeamMoneyLine[0].InnerText));
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("Error in parsing " + fileName + " " + e);
                                }
                                
                            }
                            
                        }



                        tempGame.homeTeam = homeTempTeam;
                        tempGame.awayTeam = awayTempTeam;
                        tempGame.scraper = fileName;

                        finishedGames.Add(tempGame);

                        x++;
                    }
                }

                dumpScrapedData();
            }


        }//end scrape data

        public void startPull(string sportName)
        {
            MainForm.logConsole("--Starting pull of Real Bet--");

            string url = "";

            if (sportName == "NBA")
            {
                url = "http://www.realbet.eu/sportsbook/basketball/national-basketball-association/";
            }
            else if (sportName == "NCAABasketball")
            {
                url = "http://www.realbet.eu/sportsbook/basketball/national-collegiate-athletic-association/";
            }
            else if(sportName == "MLB")
            {
                url = "http://www.realbet.eu/sportsbook/baseball/major-league-baseball/";
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
            service.LocalToRemoteUrlAccess = true;
            PhantomJSDriver driver = new PhantomJSDriver(service);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
            driver.Url = inputURL;


            string source = "";
            try
            {
                driver.Navigate();
                driver.FindElementByClassName("rotation_number");

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

