using System;

namespace SportsBetting
{
	public class Game
	{

		public Team homeTeam;
		public Team awayTeam;
		public string time;
        public string scraper;
        public Team underDog;
        public Team favorite;
		public Game ()
		{



		}



		public string dumpData()
		{
			string output = "Dumping data of game " + homeTeam.name + " VS " + awayTeam.name + Environment.NewLine;

            output += homeTeam.name + ": Line: " + homeTeam.moneyLine + Environment.NewLine;
            output += awayTeam.name + ": Line: " + awayTeam.moneyLine + Environment.NewLine;

            return output;
		}






	}
}

