using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Threading;
using System.ComponentModel;
using System.Net;
using System.Collections.Specialized;

namespace SportsBetting
{
    class WebSiteCommunication
    {

        public static string responseString;
        static string[] words;
        static string[] reservedCommands = new string[] { "sports", "scrapers"};

        public void talkToWeb()
        {
            Console.WriteLine("lukey pressed");

            string URL = "http://lukeleggio.com/arbiter/arbiterRequest.php";
            WebClient webClient = new WebClient();

            NameValueCollection formData = new NameValueCollection();
            formData["homeTeam"] = "testuser";
            formData["awayTeam"] = "mypassword";

            byte[] responseBytes = webClient.UploadValues(URL, "POST", formData);
            string responsefromserver = Encoding.UTF8.GetString(responseBytes);
            Console.WriteLine(responsefromserver);
            webClient.Dispose();
            Console.ReadKey();
            //string testing = "lukey";
            //Type testArb = this.GetType();
            //testArb.GetMethod(testing).Invoke(this, null);
        }

        public void sendGameToServer(Game inputGame)
        {
            Console.WriteLine("sending game to server...");
            string URL = "http://lukeleggio.com/arbiter/arbiterRequest.php?addNewGame";
            WebClient webClient = new WebClient();

            NameValueCollection formData = new NameValueCollection();

            formData["homeTeam"] = Helper.prepareTeamForServer(inputGame.homeTeam.name);
            formData["awayTeam"] = Helper.prepareTeamForServer(inputGame.awayTeam.name);
            formData["homeTeamLine"] = inputGame.homeTeam.moneyLine + "";
            formData["awayTeamLine"] = inputGame.awayTeam.moneyLine + "";
            formData["scraper"] = inputGame.scraper;
            formData["date"] = inputGame.time.ToString("yyyy-MM-dd HH:mm:ss"); 

            byte[] responseBytes = webClient.UploadValues(URL, "POST", formData);
            string responsefromserver = Encoding.UTF8.GetString(responseBytes);


            if(responsefromserver != "")
            {
                //Helper.writeError("Failure sending game to server", "WebSiteCom");
            }

            Console.WriteLine(responsefromserver);
            webClient.Dispose();
           

        }

        public void sendArbitrageToServer(Game inputGame1, Game inputGame2,float percentMadeOnArb)
        {
            Console.WriteLine("sending arb to server...");
            string URL = "http://lukeleggio.com/arbiter/arbiterRequest.php?addNewArbitrage";
            WebClient webClient = new WebClient();

            NameValueCollection formData = new NameValueCollection();

            formData["homeTeam1"] = Helper.prepareTeamForServer(inputGame1.homeTeam.name);
            formData["awayTeam1"] = Helper.prepareTeamForServer(inputGame1.awayTeam.name);
            formData["homeTeamLine1"] = inputGame1.homeTeam.moneyLine + "";
            formData["awayTeamLine1"] = inputGame1.awayTeam.moneyLine + "";
            formData["scraper1"] = inputGame1.scraper;
            formData["date"] = inputGame1.time.ToString("yyyy-MM-dd HH:mm:ss");

            formData["homeTeam2"] = Helper.prepareTeamForServer(inputGame2.homeTeam.name);
            formData["awayTeam2"] = Helper.prepareTeamForServer(inputGame2.awayTeam.name);
            formData["homeTeamLine2"] = inputGame2.homeTeam.moneyLine + "";
            formData["awayTeamLine2"] = inputGame2.awayTeam.moneyLine + "";
            formData["scraper2"] = inputGame2.scraper;
            formData["date2"] = inputGame1.time.ToString("yyyy-MM-dd HH:mm:ss");

            formData["percentMadeOnArb"] = percentMadeOnArb+"";

            byte[] responseBytes = webClient.UploadValues(URL, "POST", formData);
            string responsefromserver = Encoding.UTF8.GetString(responseBytes);
            Console.WriteLine(responsefromserver);
            webClient.Dispose();
           
        }


        public void sendCalendarGameToServer(string homeTeam, string awayTeam, string gameTime)
        {
            Console.WriteLine("sending calendar game to server...");
            string URL = "http://lukeleggio.com/arbiter/uploadSeasonData.php";
            WebClient webClient = new WebClient();

            NameValueCollection formData = new NameValueCollection();

            formData["homeTeam"] = homeTeam;
            formData["awayTeam"] = awayTeam;
            formData["gameTime"] = gameTime;
            

            byte[] responseBytes = webClient.UploadValues(URL, "POST", formData);
            string responsefromserver = Encoding.UTF8.GetString(responseBytes);


            if (responsefromserver != "")
            {
                //Helper.writeError("Failure sending game to server", "WebSiteCom");
            }

            Console.WriteLine(responsefromserver);
            webClient.Dispose();


        }

        public void listen()
        {
            //WebServer ws = new WebServer(SendResponse, "http://[2601:204:d300:a6fc:195c:6e37:48bd:c8a0]/");
            WebServer ws = new WebServer(SendResponse, "http://10.0.0.157:80/");
            Console.WriteLine(ws.ToString());
            ws.Run();
            Console.WriteLine("Webserver Started");
            Console.ReadKey();
            ws.Stop();
        }

        public static string SendResponse(HttpListenerRequest request)
        {
            //Console.WriteLine(request.QueryString);
            Console.WriteLine(request.RawUrl);
            parseRequest(request.RawUrl);
            //Console.WriteLine(request.ServiceName);
            return responseString;
        }


        public static void parseRequest(string input)
        {
            char[] delimiterChars = {'-','/'};
            bool startingRoutine = false;

            string arguments = input;
            words = arguments.Split(delimiterChars);
            
            
            int x = 0;
            while(x < words.Length)
            {
                
                switch(words[x])
                {
                    case "startRoutine":
                        startingRoutine = true;
                        MainClass.GlobalArbitrage.resetEverything();
                        break;
                    case "stopRoutine":
                       
                        MainClass.GlobalArbitrage.stopTimedRoutine();
                        break;
                    case "sports":
                        x = addSports(x+1);

                        break;
                    case "scrapers":
                        x = addScraper(x + 1);

                        break;
                    case "isRoutineRunning":
                        Console.WriteLine("Checking if routine is running...");
                        MainClass.GlobalArbitrage.isRoutineRunning();

                        break;
                }

                x++;
            }

            if(startingRoutine)
            {
                //MainClass.GlobalArbitrage.startEverything();
                MainClass.GlobalArbitrage.startTimedRoutine(20);
            }
            //switch (input)
            //{
            //    case "/startRoutine":

            //        Console.WriteLine("WebRequest: StartTimedRoutine");
            //        MainClass.GlobalArbitrage.startTimedRoutine(1000);

            //        break;
            //    case "/stopRoutine":

            //        Console.WriteLine("Web Request: StopTimedRoutine");
            //        MainClass.GlobalArbitrage.stopTimedRoutine();

            //        break;
            //    case "/isRoutineRunning":

            //        Console.WriteLine("Web Request: isRoutineRunning");
            //        MainClass.GlobalArbitrage.isRoutineRunning();

            //        break;
            //    default:
            //        Console.WriteLine("Web Request: couldn't parse");
            //        responseString = "bad request";
            //        break;
            //}




            MainClass.GlobalArbitrage.dumpTestData();




            
        }


        public static int addSports(int inputPos)
        {
            int x = inputPos;
            while (x < words.Length)
            {
                if(!checkReservedCommands(words[x]))
                {
                    MainClass.GlobalArbitrage.addSport(words[x]);
                }
                else
                {
                    return x-1;
                }

                x++;
            }

            return inputPos;
        }

        public static int addScraper(int inputPos)
        {
            int x = inputPos;
            while (x < words.Length)
            {
                if (!checkReservedCommands(words[x]))
                {
                    MainClass.GlobalArbitrage.addScraper(words[x]);
                }
                else
                {
                    return x-1;
                }

                x++;
            }

            return inputPos;
        }

        public static bool checkReservedCommands(string input)
        {

            foreach(string com in reservedCommands)
            {
                if(input == com)
                {
                    return true;
                }
            }

            return false;
        }
    }

   
}
