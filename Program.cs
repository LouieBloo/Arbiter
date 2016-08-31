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
        public static string serverIP = "";
      
       
        public static void Main (string[] args)
		{


            //Console.WriteLine(GlobalMainForm.Controls["MainDisplay"].Text = "uhhh");
            //Application.Run(GlobalMainForm); 

            // This starts everything up

            // Decided to not do accounts. All accounts will be processed with php
            //AccountManager accountManager = new AccountManager();

            try
            {
                string[] lines = System.IO.File.ReadAllLines(@"..\..\config.txt");
                foreach (string line in lines)
                {
                    // Use a tab to indent each line of the file.
                    Console.WriteLine("Found ip to use: " + line);
                    serverIP = line;
                    GlobalArbitrage.startServer();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            

           




            

            //Helper.textMessage("9166225360@txt.att.net","Luke","Whats up dude!", "Testing shit out, you know");






            //GlobalArbitrage.addScraper("SportsBook.ag");
            //GlobalArbitrage.addScraper("BetDSI.eu");
            //GlobalArbitrage.addScraper("RealBet.eu");
            // GlobalArbitrage.addScraper("MyBookie.ag");
            GlobalArbitrage.addSport("MLB");           
            GlobalArbitrage.currentSport = "MLB";
            //GlobalArbitrage.startFullSportArbitrage();
            //GlobalArbitrage.scrapeAllSites();
          
            Console.ReadKey();

        }



		


		
	}
}
