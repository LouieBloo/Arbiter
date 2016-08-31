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
class AccountManager
{
        List<Account> activeAccounts = new List<Account>();


        public AccountManager()
        {

        }

        public Account createAccount(int uniqueID, string accountName, string phoneAddress)
        {
            Account tempAccount = new Account();

            tempAccount.setup(uniqueID, accountName, phoneAddress);


            return tempAccount;
        }

        public void initializeAccounts()
        {
            createAccount(1, "Luke", "9166225360@txt.att.net");
            createAccount(2, "Dunn", "9163006143@vtext.com");
        }


        public void checkIfAnAccountIsListeningForThisGame(string homeTeam, string awayTeam)
        {
            foreach(Account acc in activeAccounts)
            {
                //if(acc.listeningHomeTeam == homeTeam && listeningAwayTeam == awayTeam && list)
            }
        }
        

}
}// End namespace
