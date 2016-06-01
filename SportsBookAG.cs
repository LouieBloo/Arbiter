using System;
using System.Text;
using System.Collections.Generic;
using HtmlAgilityPack;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.Extensions;
using WatiN.Core;
using System.ComponentModel;

namespace SportsBetting
{
	public class SportsBookAG: WebsiteScraper
	{
        string fileName = "SportsBook.ag";
        List<Game> finishedGames = new List<Game>();
        bool queued = false;

		public SportsBookAG ()
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

            HtmlNodeCollection nodes = document2.DocumentNode.SelectNodes ("//*[contains(concat(\" \", normalize-space(@class), \" \"), \" eventbox \")]"); 
			if (nodes == null) 
			{
				Helper.writeError("Couldn't find any rows! " + sport,fileName);
			} 
			else {

				foreach (HtmlNode item in nodes) {  

					Game tempGame = new Game ();
					Team homeTempTeam = new Team ();
					Team awayTempTeam = new Team ();
                    DateTime timeStamp;

                    //Get Time
                    HtmlNodeCollection times = item.SelectNodes (".//div[@id='time']");
                    
					if (times [0] != null) {

                        string time = times[0].InnerText.Replace("\t", "").Replace("\n", "").Replace(" ", "").Replace("\r", "").Replace(".", "").Replace("EDT","").ToString();
                        timeStamp = DateTime.Parse(time);//making it in edt not pac
                        tempGame.time = timeStamp;
                    }
                    else
					{
                        Helper.writeError("Couldn't find time! ", fileName + sport);
                    }

					//Get Name For each team
					HtmlNodeCollection names = item.SelectNodes (".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" team-title \")]");
					if (names.Count == 2) {

						string nameHome = names [0].InnerText.Replace("\t","").Replace ("\n", "").Replace (" ", "-").Replace ("\r", "").Replace(".", "");
						string nameAway = names [1].InnerText.Replace("\t","").Replace ("\n", "").Replace (" ", "-").Replace ("\r", "").Replace(".", "");
						nameHome = nameHome.ToUpper ();
						nameAway = nameAway.ToUpper ();

						homeTempTeam.name = Helper.replaceTeamName(nameHome,fileName);
						awayTempTeam.name = Helper.replaceTeamName(nameAway,fileName);

					}
					else
					{
                        Helper.writeError("Too many or not enough names found!", fileName + sport);
                    }

					//Get Money For each team
					HtmlNodeCollection money = item.SelectNodes (".//*[contains(concat(\" \", normalize-space(@class), \" \"), \" total \")]");
					if (money.Count == 2) {

						string moneyLineHomeString = money [0].InnerText.Replace("\t","").Replace ("\n", "").Replace (" ", "-").Replace ("\r", "");
						string moneyLineAwayString = money [1].InnerText.Replace("\t","").Replace ("\n", "").Replace (" ", "-").Replace ("\r", "");

						moneyLineHomeString = Helper.parseMoneyLine (moneyLineHomeString, moneyLineAwayString);
						moneyLineAwayString = Helper.parseMoneyLine (moneyLineAwayString, moneyLineHomeString);

                        try
                        {
                            homeTempTeam.moneyLine = Int32.Parse(moneyLineHomeString);
                            awayTempTeam.moneyLine = Int32.Parse(moneyLineAwayString);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Error in parsing " + fileName + " " + e);
                        }
                        

					}
					else
					{
                        Helper.writeError("Too many or too little money lines!", fileName + sport);
                    }

					tempGame.homeTeam = homeTempTeam;
					tempGame.awayTeam = awayTempTeam;
                    tempGame.scraper = fileName;
                    //tempGame.time = timeStamp;time added above

					finishedGames.Add (tempGame);
				}

                dumpScrapedData();
			}


		}//end scrape data


        public void startPull(string sportName)
        {
            MainForm.logConsole("--Starting pull of SportsBook--");

            string url = "";

            if(sportName == "NBA")
            {
                url = "https://www.sportsbook.ag/sbk/sportsbook4/nba-betting/nba-game-lines.sbk";
            }
            else if(sportName == "NCAABasketball")
            {
                url = "https://www.sportsbook.ag/sbk/sportsbook4/ncaa-basketball-betting/ncaa-basketball-lines.sbk";
            }
            else if(sportName == "MLB")
            {
                url = "https://www.sportsbook.ag/sbk/sportsbook4/baseball-betting/mlb-lines.sbk";
            }
            else
            {

            }

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (obj, e) => pullData(url, sportName);
            bw.RunWorkerCompleted += (obj, e) => MainClass.GlobalArbitrage.finishedAPull(this);
            bw.RunWorkerAsync();
        
        }

        void pullData(string inputURL, string sportName)
        {
            Console.WriteLine("Starting Pull of " + inputURL);
            PhantomJSDriverService service = PhantomJSDriverService.CreateDefaultService("E:\\SportsBetting\\packages\\PhantomJS.2.1.1\\tools\\phantomjs");
            service.IgnoreSslErrors = true;
            service.SslProtocol = "any";
            service.LocalToRemoteUrlAccess = true;
            //service.AddArgument("--ignore-ssl-errors=true");
            // service.AddArgument("--ssl-protocol=tlsv1");
            //service.GhostDriverPath = "E:\\SportsBetting\\packages\\PhantomJS.2.1.1\\tools\\phantomjs";
            //PhantomJSDriver driver = new PhantomJSDriver("E:\\SportsBetting\\packages\\PhantomJS.2.1.1\\tools\\phantomjs");
            PhantomJSDriver driver = new PhantomJSDriver(service);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(20));
            //driver.Url = inputURL;

            string source = "";
            try
            {
                driver.Navigate().GoToUrl(inputURL);
                source = driver.PageSource;
            }
            catch (Exception e)
            {
                Helper.writeError(e.ToString(), (fileName + sportName));
            }

            Helper.writePulledData(source, (fileName+sportName));
            driver.Quit();

            Console.WriteLine("Data pulled");
        }

        void dumpScrapedData()
        {
            string final = "";
            foreach(Game gm in finishedGames)
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

