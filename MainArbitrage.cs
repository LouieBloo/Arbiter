using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using HtmlAgilityPack;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Support.Events;
using OpenQA.Selenium.Support.Extensions;
using WatiN.Core;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.ComponentModel;
using System.Net;
using System.Collections.Specialized;
using System.Timers;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SportsBetting
{
    class MainArbitrage
    {
        
        int currentScraperIndex = 0;
        int currentSportIndex = 0;
        public string currentSport = "";

        public bool arbitrageBusy = false;

        public System.Timers.Timer routineTimer = new System.Timers.Timer();

        List<WebsiteScraper> activeScrapers = new List<WebsiteScraper>();
        List<string> activeSports = new List<string>();

        public WebSiteCommunication webCom;

        public MainArbitrage()
        {
            WebsiteScraper sportsAG = new SportsBookAG();
            WebsiteScraper realBet = new RealBet();
            WebsiteScraper myBookie = new MyBookie();
            WebsiteScraper betDSI = new BetDSI();

            activeScrapers.Add(sportsAG);
            activeScrapers.Add(realBet);
            activeScrapers.Add(myBookie);
            activeScrapers.Add(betDSI);

            webCom = new WebSiteCommunication();
        }

        public void startFullSportArbitrage()
        {
            Console.WriteLine("--Starting All Pulls and Scrapes--");
            Console.WriteLine("--currentScraper index = " + currentScraperIndex);
            if(activeSports.Count() > currentSportIndex)
            {
                arbitrageBusy = true;
               

                
                //currentSport = "NBA";
                currentSport = activeSports[currentSportIndex];
                startNextScraper();
            }
            
            
        }

        private void startNextScraper()
        {
            
            Console.WriteLine("--Starting next Scraper--");
            Console.WriteLine(currentScraperIndex + " - " + currentSport);
            Console.WriteLine("--Active scraper count " + activeScrapers.Count());

            if (currentScraperIndex >= activeScrapers.Count)
            {
                Console.WriteLine("Current Scraper index > activeScrapers: scraping all sites...");
                scrapeAllSites();
            }
            else
            {
                if (activeScrapers[currentScraperIndex].isQueued())
                {
                    Console.WriteLine("Found Queued Scraper...");
                    activeScrapers[currentScraperIndex].startPull(currentSport);
                }
                else
                {
                    Console.WriteLine("current scraper isnt queued to pull...");
                    currentScraperIndex++;
                    startNextScraper();
                }
            }

           
            Console.WriteLine("startNextScraper finished...");

        }
        
        public void finishedAPull(WebsiteScraper inputScraper)
        {
            Console.WriteLine("--Finished pull of " + inputScraper.getName() + "--");

            currentScraperIndex++;
            startNextScraper();
            
        }

        public void scrapeAllSites()
        {
            Console.WriteLine("--All Sites Pulled--");
            Console.WriteLine("--Starting Scrape on all pulled Data--");
            foreach (WebsiteScraper web in activeScrapers)
            {
                if (web.isQueued())
                {
                    Console.WriteLine("Current Sport before scrape: " + currentSport); 
                    web.scrapeData(currentSport);
                }
                
            }
            Console.WriteLine("--Finished Scraping--");

            compareGames();
        }


        private void compareGames()
        {
            ArbiterCompare arb = new ArbiterCompare();

            arb.startComparing(activeScrapers);

            //send games to server
            foreach(WebsiteScraper web in activeScrapers)
            {
                if(web.isQueued())
                {
                    foreach(Game gm in web.getGames())
                    {
                        webCom.sendGameToServer(gm);
                    }
                    
                }
            }


            
            arbitrageBusy = false;

            finishedSport();
            
        }

        private void finishedSport()
        {
            Console.WriteLine("Finished a sport: " + activeSports[currentSportIndex]);

            currentSportIndex++;

            if(currentSportIndex < activeSports.Count())
            {
                Console.WriteLine("More sports to arbitrage...");
                
                currentScraperIndex = 0;
                startFullSportArbitrage();
            }
            else
            {
                resetForNextTimedRoutine();
                Console.WriteLine("-----Finished a routine--------");
            }
            
            
            
        }

        

        

        public void startTimedRoutine(float intervalInMinutes)
        {
            routineTimer.Stop();
            routineTimer.Enabled = false;

            routineTimer = new System.Timers.Timer();

            routineTimer.Elapsed += new ElapsedEventHandler(routineTimerEvent);
            routineTimer.Interval = intervalInMinutes * 60000;
            routineTimer.Enabled = true;



            WebSiteCommunication.responseString = "ok";

            startFullSportArbitrage();
        }

        public void stopTimedRoutine()
        {
            routineTimer.Stop();
            routineTimer.Enabled = false;

            WebSiteCommunication.responseString = "ok";
        }

        public bool isRoutineRunning()
        {
            if(routineTimer.Enabled)
            {
                WebSiteCommunication.responseString = "Routine Running";
                return true;
            }
            WebSiteCommunication.responseString = "Routine Not Running";
            return false;
        }

        public  void routineTimerEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Scheduled Routine Starting...");
            startFullSportArbitrage();
        }

        public void addScraper(string input)
        {
            foreach(WebsiteScraper web in activeScrapers)
            {
                if(web.getName() == input)
                {
                    web.setQueued(true);
                    break;
                }
            }
        }

        public void addSport(string input)
        {
            activeSports.Add(input);
        }

        //resets the entire arbitrage and stopping any running routines.
        public void resetEverything()
        {
            stopTimedRoutine();
            
            currentScraperIndex = 0;
            currentSport = "";
            currentSportIndex = 0;

            arbitrageBusy = false;
            activeSports = new List<string>();

            foreach(WebsiteScraper web in activeScrapers)
            {
                web.setQueued(false);
            }
        }

        void resetForNextTimedRoutine()
        {
            currentScraperIndex = 0;
            currentSport = "";
            currentSportIndex = 0;

            arbitrageBusy = false;
        }

        private void startServer()
        {
            
            webCom.listen();

            //Console.WriteLine("/startRoutine-sports-NBA-MLB-scrapers-RealBet-MyBookie");
            //WebSiteCommunication.parseRequest("/startRoutine-sports-NBA-MLB-scrapers-RealBet-MyBookie");
            //Console.ReadKey();
        }


        public void dumpTestData()
        {

            Console.WriteLine("Dumping MainArb data:");
            foreach (WebsiteScraper web in activeScrapers)
            {
                Console.WriteLine(web.isQueued());
            }

            foreach(string s in activeSports)
            {
                Console.WriteLine(s);
            }
        }


        public void testPull()
        {
            //currentScraperIndex = -1;
            //currentSport = "MLB";

            //allScrapers[0].startPull(currentSport);//Sports
            //allScrapers[1].startPull("NBA");//RealBet
            //allScrapers[2].startPull("NBA");//MyBookie
            //allScrapers[3].scrapeData(currentSport);
            //allScrapers[3].startPull(currentSport);
            //scrapeAllSites();
            //Console.ReadKey();
            //startTimedRoutine(1000);

            Helper.deleteAllLogs();
            startServer();


            //startEverything();
            //pullData("https://www.betdsi.eu/sportsbook-betting/", "NBA");

        }

        


        

    }
}
