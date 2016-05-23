using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;
using System.IO;

namespace SportsBetting
{
    class Helper
    {
        
        public static string logPath = "e:\\SportsBetting\\Logs\\";

        public static void textMessage(string inputAddress)
        {
            MailMessage msg = new MailMessage();
            //msg.To.Add(new MailAddress("9166225360@txt.att.net", "SomeOne"));
            msg.To.Add(new MailAddress("9163006143@vtext.com", "Zac"));
            msg.From = new MailAddress("leggioluke5@gmail.com", "Arbiter");
            msg.Subject = "";
            //msg.Body = "Hello Zac. My name is Arbiter! Luke wanted to test out if I can send text messages and it turns out I can! Woo hoo!";
            msg.Body = "Hey Nick, you are the coolest! Stay fresh brotha!";
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("leggioluke5@gmail.com", "pu9c3nodw");
            client.Port =25; // You can use Port 25 if 587 is blocked (mine is!)
            client.Host = "smtp.gmail.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            try
            {
                client.Send(msg);
                Console.WriteLine( "Message Sent Succesfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static void dumpASCII(string input)
        {
            byte[] ASCIIValues = Encoding.ASCII.GetBytes(input);
            foreach (byte b in ASCIIValues)
            {
                Console.WriteLine(b);
            }
        }

        public static float calculateDecimalOdds(int moneyLine)
        {
            if(moneyLine < 0)
            {
                
                float returnVal = (1 + (100 / (float)Math.Abs(moneyLine)));
                
                return returnVal;
            }
            else
            {
                float returnVal = 1 + ((float)moneyLine / 100);
               
                return returnVal;
            }
        }

        //parses input1 based on input2
        public static string parseMoneyLine(string input1, string input2)
        {
            if (input1 == "-" || input1 == "")
            {
                return "0";
            }


            if (String.Compare(input1, "even", StringComparison.OrdinalIgnoreCase) == 0 || String.Compare(input1, "ev", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (String.Compare(input2, "even", StringComparison.OrdinalIgnoreCase) == 0 || String.Compare(input2, "ev", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    Console.WriteLine("IDK WHAT TO DO HERE!");
                    return "0";
                }
                else {
                    int holder = Int32.Parse(input2);
                    int temp = holder * -100;
                    temp /= Math.Abs(holder);
                    return (temp + "");
                }

            }

            return input1;

        }


        public static string replaceTeamName(string input,string website)
        {

            string temp = input;
           
            //GET RID OF EXCESS - IN TEAM NAME
            foreach (char c in input)
            {
                if(c == '-')
                {
                    //Console.WriteLine("here");
                    temp = temp.Remove(0, 1);
                    //Console.WriteLine(temp);
                }
                else
                {
                    break;
                }
            }

            for (int i = input.Length; i-- > 0;)
            {
                if(input[i] == '-')
                {
                    temp = temp.Remove(temp.Length-1, 1);
                }
                else
                {
                    break;
                }
            }
           
            //temp = ReplaceFirst(temp,"LOS-ANGELES", "LA");

            if(!temp.Contains("DIAMONDBACKS"))
            {
                temp = ReplaceFirst(temp, "DBACKS", "DIAMONDBACKS");
            }
            if (!temp.Contains("SAN-FRANCISCO"))
            {
                temp = ReplaceFirst(temp, "SAN-FRAN", "SAN-FRANCISCO");
            }
            if (!temp.Contains("PHILADELPHIA"))
            {
                temp = ReplaceFirst(temp, "PHILA", "PHILADELPHIA");
            }

        
            
            if (MainClass.GlobalArbitrage.currentSport == "MLB")
            {

                
                int x = 0;
                while(x < 9)
                {
                    if (temp.Contains("-(GM-"+x+")"))
                    {
                        temp = ReplaceFirst(temp, "-(GM-" + x + ")", "");
                    }

                    x++;
                }
                //need to work on mybookie multiple games for double header.







                string firstThree = temp.Substring(0, 3);
                string firstTwo = temp.Substring(0, 2);
                
                string original = temp;
               
                switch (firstThree)
                {
                    case "PHI":
                        if (!temp.Contains("PHILADELPHIA"))
                        {
                            temp = ReplaceFirst(temp, "PHI", "PHILADELPHIA");
                        }
                        break;
                    case "WAS":
                        if (!temp.Contains("WASHINGTON"))
                        {
                            temp = ReplaceFirst(temp, "WAS", "WASHINGTON");
                        }
                        break;
                    case "CIN":
                        if (!temp.Contains("CINCINNATI"))
                        {
                            temp = ReplaceFirst(temp, "CIN", "CINCINNATI");
                        }
                        break;
                    case "MIL":
                        if (!temp.Contains("MILWAUKEE"))
                        {
                            temp = ReplaceFirst(temp, "MIL", "MILWAUKEE");
                        }
                        break;
                    case "CHI":
                        if (!temp.Contains("CHICAGO"))
                        {
                            temp = ReplaceFirst(temp, "CHI", "CHICAGO");
                        }
                        break;
                    case "PIT":
                        if (!temp.Contains("PITTSBURGH"))
                        {
                            temp = ReplaceFirst(temp, "PIT", "PITTSBURGH");
                        }
                        break;
                    case "COL":
                        if (!temp.Contains("COLORADO"))
                        {
                            temp = ReplaceFirst(temp, "COL", "COLORADO");
                        }
                        break;
                    case "STL":
                        if (!temp.Contains("ST-LOUIS"))
                        {
                            temp = ReplaceFirst(temp, "STL", "ST-LOUIS");
                        }
                        break;
                    case "ARI":
                        if (!temp.Contains("ARIZONA"))
                        {
                            temp = ReplaceFirst(temp, "ARI", "ARIZONA");
                        }
                        break;
                    case "MIA":
                        if (temp.Contains("MIAMI-MARLINS"))
                        {
                            
                        }
                        else if (!temp.Contains("MIA-MARLINS"))
                        {
                            temp = ReplaceFirst(temp, "MIA","MIAMI-MARLINS");
                        }
                        else if(!temp.Contains("MIAMI"))
                        {
                            temp = ReplaceFirst(temp, "MIA", "MIAMI");
                        }
                        break;
                    case "SDG":
                        if (!temp.Contains("SAN-DIEGO"))
                        {
                            temp = ReplaceFirst(temp, "SDG", "SAN-DIEGO");
                        }
                        break;
                    case "SFO":
                        if (!temp.Contains("SAN-FRANCISCO"))
                        {
                            temp = ReplaceFirst(temp, "SFO", "SAN-FRANCISCO");
                        }
                        break;
                    case "TOR":
                        if (!temp.Contains("TORONTO"))
                        {
                            temp = ReplaceFirst(temp, "TOR", "TORONTO");
                        }
                        break;
                    case "OAK":
                        if (!temp.Contains("OAKLAND"))
                        {
                            temp = ReplaceFirst(temp, "OAK", "OAKLAND");
                        }
                        break;
                    case "DET":
                        if (!temp.Contains("DETROIT"))
                        {
                            temp = ReplaceFirst(temp, "DET", "DETROIT");
                        }
                        break;
                    case "BAL":
                        if (!temp.Contains("BALTIMORE"))
                        {
                            temp = ReplaceFirst(temp, "BAL", "BALTIMORE");
                        }
                        break;
                    case "TEX":
                        if (!temp.Contains("TEXAS"))
                        {
                            temp = ReplaceFirst(temp, "TEX", "TEXAS");
                        }
                        break;
                    case "CLE":
                        if (!temp.Contains("CLEVELAND"))
                        {
                            temp = ReplaceFirst(temp, "CLE", "CLEVELAND");
                        }
                        break;
                    case "MIN":
                        if (!temp.Contains("MINNESOTA"))
                        {
                            temp = ReplaceFirst(temp, "MIN", "MINNESOTA");
                        }
                        break;
                    case "HOU":
                        if (!temp.Contains("HOUSTON"))
                        {
                            temp = ReplaceFirst(temp, "HOU", "HOUSTON");
                        }
                        break;
                    case "SEA":
                        if (!temp.Contains("SEATTLE"))
                        {
                            temp = ReplaceFirst(temp, "SEA", "SEATTLE");
                        }
                        break;
                    case "BOS":
                        if (!temp.Contains("BOSTON"))
                        {
                            temp = ReplaceFirst(temp, "BOS", "BOSTON");
                        }
                        break;
                    case "ATL":
                        if (!temp.Contains("ATLANTA"))
                        {
                            temp = ReplaceFirst(temp, "ATL", "ATLANTA");
                        }
                        break;
                    case "KAN":
                        if (!temp.Contains("KANSAS-CITY"))
                        {
                            temp = ReplaceFirst(temp, "KAN", "KANSAS-CITY");
                        }
                        break;
                    case "TAM":
                        if (!temp.Contains("TAMPA-BAY"))
                        {
                            temp = ReplaceFirst(temp, "TAM", "TAMPA-BAY");
                        }
                        break;

                }

                if(temp == original)//only check 2 letters if we didnt find a match in 3 letters
                {
                    switch (firstTwo)
                    {
                        case "NY":
                            if (!temp.Contains("NEW-YORK"))
                            {
                                temp = ReplaceFirst(temp, "NY", "NEW-YORK");
                            }
                            break;
                        case "TB":
                            if (!temp.Contains("TAMPA-BAY"))
                            {
                                temp = ReplaceFirst(temp, "TB", "TAMPA-BAY");
                            }
                            break;
                        case "KC":
                            if (!temp.Contains("KANSAS-CITY"))
                            {
                                temp = ReplaceFirst(temp, "KC", "KANSAS-CITY");
                            }
                            break;
                        case "LA":
                            if (!temp.Contains("LOS-ANGELES"))
                            {
                                temp = ReplaceFirst(temp, "LA", "LOS-ANGELES");
                            }
                            break;
                    }
                }


            }


            return temp;
        }

        
        public static string prepareTeamForServer(string input)
        {
            return input.Replace('-', ' ');
        }

        public static void writeError(string inputString, string fileName)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(logPath + "Errors\\" + fileName + DateTime.Now.ToString().Replace('/', '-').Replace(':', '.') + ".txt");
            file.WriteLine(inputString);

            file.Close();
        }

        public static string writePulledData(string inputString, string fileName)
        {
            string returnedData = logPath + "PulledData\\" + fileName + "Pulled" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '.') + ".txt";

            System.IO.StreamWriter workingFile = new System.IO.StreamWriter(logPath + fileName + "Pulled" + ".txt");
            System.IO.StreamWriter storedFile = new System.IO.StreamWriter(returnedData);
            workingFile.WriteLine(inputString);
            storedFile.WriteLine(inputString);

            workingFile.Close();
            storedFile.Close();


            return returnedData;
        }

        public static void writeScrapedData(string inputString, string fileName)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(logPath + "ScrapedData\\" + fileName + "Scraped" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '.') + ".txt");
            file.WriteLine(inputString);

            file.Close();
        }

        public static void writeArbitrage(string fileName, string inputString)
        {
    
            System.IO.StreamWriter file = new System.IO.StreamWriter(logPath + "Arbitrages\\" + fileName + "Arb" + DateTime.Now.ToString().Replace('/', '-').Replace(':', '.') + ".txt");
            file.WriteLine(inputString);

            file.Close();
        }


        public static void deleteAllLogs()
        {
            deleteLogFromFolder(logPath);
            deleteLogFromFolder(logPath + "Errors\\");
            deleteLogFromFolder(logPath + "PulledData\\");
            deleteLogFromFolder(logPath + "ScrapedData\\");
            MainForm.logConsole("--Logs Deleted--");
        }

        static void deleteLogFromFolder(string input)
        {
            DirectoryInfo di = new DirectoryInfo(@input);
            FileInfo[] files = di.GetFiles("*.txt")
                                 .Where(p => p.Extension == ".txt").ToArray();
            foreach (FileInfo file in files)
                try
                {
                    file.Attributes = FileAttributes.Normal;
                    File.Delete(file.FullName);
                }
                catch {
                    Console.WriteLine("Something went wrong deleting logs");
                }
        }

        public static bool nameMatch(string sport, string teamOne, string teamTwo)
        {

            if(teamOne.Contains(teamTwo) || teamTwo.Contains(teamOne))
            {
                return true;
            }


            return false;
        }


        public static string ReplaceFirst(string text, string search, string replace)
        {
            int pos = text.IndexOf(search);
            if (pos < 0)
            {
                return text;
            }
            return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
        }
    }


   
}
