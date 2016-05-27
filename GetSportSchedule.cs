using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SportsBetting
{


    
    class GetSportSchedule
    {



        //http://www.msn.com/en-us/sports/mlb/schedule
        //this one sends everything to the server, read from a txt file.
        public void bullshitPull()
        {
            int counter = 0;
            string line;
            string output = "";

            int currentMonth = 0;
            int currentDay = 0;
            string currentTime = "";

            string homeTeam = "";
            string awayTeam = "";
            bool midGame = false;

            char[] delim = { '*' };
            int noth = 0;

            WebSiteCommunication web = new WebSiteCommunication();

            System.IO.StreamReader file = new System.IO.StreamReader("e:\\SportsBetting\\mlbOutput.txt");
            while ((line = file.ReadLine()) != null)
            {

                string[] temp = line.Split(delim);
                //string[] temp = Regex.Split(line, "[ | ]");

                //Console.WriteLine(line + "uh");
                if (temp.Count() > 2)
                {
                    Console.WriteLine(temp[0] + temp[1] + temp[2]);
                    web.sendCalendarGameToServer(temp[0], temp[1], temp[2]);
                }



            }

            file.Close();


            // DateTime test = new DateTime(DateTime.Now.Year,4,1, DateTime.Parse(testTime).Hour, DateTime.Parse(testTime).Minute, DateTime.Parse(testTime).Second);

            // Console.WriteLine(test);
            //Console.WriteLine(test.ToString("yyyy-MM-dd HH:mm:ss"));




            //Console.WriteLine(test);
            Console.ReadLine();

        }

        //this was to get the sport schdeule in the format i wanted. doesnt work, needs to be looked at but i thought
        // id save it here
        void doesntWork()
        {
            int counter = 0;
            string line;
            string output = "";

            int currentMonth = 0;
            int currentDay = 0;
            string currentTime = "";

            string homeTeam = "";
            string awayTeam = "";
            bool midGame = false;

            char[] delim = { ' ' };
            int noth = 0;



            System.IO.StreamReader file = new System.IO.StreamReader("e:\\SportsBetting\\mlbOutput.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (line.Length < 3 || line.Contains("@"))
                {

                }
                else if (line.Contains("date"))
                {
                    //string[] temp = line.Split(delim);
                    string[] temp = Regex.Split(line, " | ");
                    currentMonth = int.Parse(temp[1]);
                    currentDay = int.Parse(temp[2]);
                }
                else if (int.TryParse(line[0] + "", out noth))
                {
                    currentTime = line;

                }
                else
                {
                    if (!midGame)
                    {
                        homeTeam = line;
                        midGame = true;
                    }
                    else
                    {
                        awayTeam = line;

                        DateTime test = new DateTime(DateTime.Now.Year, currentMonth, currentDay, DateTime.Parse(currentTime).Hour, DateTime.Parse(currentTime).Minute, DateTime.Parse(currentTime).Second);
                        output += homeTeam + " | " + awayTeam + " | " + test.ToString("yyyy-MM-dd HH:mm:ss") + Environment.NewLine;
                        midGame = false;
                    }
                }

                counter++;
            }

            file.Close();


            // Write the string to a file.
            System.IO.StreamWriter file2 = new System.IO.StreamWriter("e:\\SportsBetting\\mlbOutput.txt");
            file2.WriteLine(output);

            file2.Close();

            //// Suspend the screen.
            //Console.ReadLine();




            string testDate = "Jun 1";
            string testTime = "8:10 PM";



            // DateTime test = new DateTime(DateTime.Now.Year,4,1, DateTime.Parse(testTime).Hour, DateTime.Parse(testTime).Minute, DateTime.Parse(testTime).Second);

            // Console.WriteLine(test);
            //Console.WriteLine(test.ToString("yyyy-MM-dd HH:mm:ss"));




            //Console.WriteLine(test);
            Console.ReadLine();
        }


    }
}
