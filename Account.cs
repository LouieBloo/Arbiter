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
    class Account
    {
        int uniqueID;
        string accountName;
        string phoneAddress;

        public Account()
        {

        }

        public void setup(int inputID, string inputAccountName, string inputPhoneAddress)
        {
            uniqueID = inputID;
            accountName = inputAccountName;
            phoneAddress = inputPhoneAddress;
        }



    }
}
