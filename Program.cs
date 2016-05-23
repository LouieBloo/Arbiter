using System;
using System.Text;
using System.Collections.Generic;
using HtmlAgilityPack;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.Extensions;
using WatiN.Core;
using System.Windows.Forms;

namespace SportsBetting
{
	class MainClass
	{

        public static MainArbitrage GlobalArbitrage = new MainArbitrage();
        public static MainForm GlobalMainForm = new MainForm();

        public static void Main (string[] args)
		{


            //Console.WriteLine(GlobalMainForm.Controls["MainDisplay"].Text = "uhhh");
            //Application.Run(GlobalMainForm); 


            //GlobalArbitrage.testPull();

            WebSiteCommunication web = new WebSiteCommunication();
            //web.talkToWeb();
            Game test = new Game();
            Team h = new Team();
            Team a = new Team();
            h.name = "Baltimore Orioles";
            h.moneyLine = 45;
            a.name = "Boston Red Sox";
            a.moneyLine = 123;
            test.homeTeam = h;
            test.awayTeam = a;
            test.scraper = "testScraper";
            //web.sendGameToServer(test);


            GlobalArbitrage.addScraper("SportsBook.ag");
            GlobalArbitrage.addScraper("BetDSI.eu");
            GlobalArbitrage.addScraper("RealBet.eu");
            GlobalArbitrage.addScraper("MyBookie.ag");
            GlobalArbitrage.addSport("MLB");
            //GlobalArbitrage.startFullSportArbitrage();
           GlobalArbitrage.currentSport = "MLB";
          GlobalArbitrage.scrapeAllSites();
            Console.ReadKey();

        }



		


		
	}
}
