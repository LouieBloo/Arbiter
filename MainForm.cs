using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SportsBetting
{
    public partial class MainForm : Form
    {
        int currentTime = 0;
        
        public MainForm()
        {
            
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //MainClass.GlobalArbitrage.startPull();
            //MainClass.GlobalArbitrage.testPull();
            //MainClass.GlobalArbitrage.startScraping();
            //Helper.textMessage("9166225360@txt.att.net");  
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        public static void logConsole(string input)
        {
            // logBox += input + Environment.NewLine;

            //MainClass.GlobalMainForm.Controls["MainDisplay"].Text += input + Environment.NewLine;
            //MainClass.GlobalMainForm.Controls["MainDisplay"].SelectionStart = logBox.Text.Length;
           // MainClass.GlobalMainForm.Controls["MainDisplay"].ScrollToCaret();

        }

        public static void logBox1(string input)
        {
            MainClass.GlobalMainForm.Controls["Output1"].Text += input + Environment.NewLine;

        }

        public static void logBox2(string input)
        {
            MainClass.GlobalMainForm.Controls["Output2"].Text += input + Environment.NewLine;

        }

        private void MainDisplay_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Helper.deleteAllLogs();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //currentTime++;

            if(currentTime >= (90) && !MainClass.GlobalArbitrage.arbitrageBusy)
            {
                MainClass.GlobalMainForm.Controls["MainDisplay"].Text = "";
                Console.Clear();
                MainClass.GlobalArbitrage.startFullSportArbitrage();
                currentTime = 0;
                
            }
            
            
        }
    }
}
