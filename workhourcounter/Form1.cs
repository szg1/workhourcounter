using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace workhourcounter
{
    public partial class whc : Form
    {


        DateTime startd;
        DateTime endd;
        TimeSpan wits;
        bool working = false;
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString() + "\\cansatworkhours.log";

        public void dptime()
        {
            DateTime now = DateTime.Now;

            textBox1.Text = now.ToString();
            //remove last element of sl

            if (working)
            {
                wits = TimeSpan.Parse(String.Join(".", (now - startd).ToString().Split('.').SkipLast(1).ToArray()));
                textBox2.Text="Worked in this session:\t"+wits.ToString();
            }


        }
        private void fio()
        {
            String wh = String.Join(".", (endd - startd).ToString().Split('.').SkipLast(1).ToArray());
            
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.Write(startd.ToString());
                sw.Write('\t');
                sw.Write(endd.ToString());
                sw.Write('\t');
                sw.Write(wh);
                sw.Write('\n');
            }
        }
        
        private String fii(bool today)
        {
            String retval = "";
            if(today)
            {
                string[] alldata = File.ReadAllLines(path);
                TimeSpan alltime = TimeSpan.Parse("00:00:00");
                foreach (String s in alldata)
                {
                    if (DateTime.Parse(s.Split('\t')[0]).Date == DateTime.Today)
                    alltime += TimeSpan.Parse(s.Split('\t')[2]);
                }
                retval = alltime.ToString();
            }
            else
            {
                string[] alldata = File.ReadAllLines(path);
                TimeSpan alltime = TimeSpan.Parse("00:00:00");
                foreach(String s in alldata)
                {
                    alltime += TimeSpan.Parse(s.Split('\t')[2]);
                }
                retval = alltime.ToString();
            }
            return retval;
        }
        public whc()
        {
            InitializeComponent();
            timer1.Start();
            StillRunning.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (working)
            {
                endd = DateTime.Now;
                working = false;
                fio();
                button1.Text = "Start work";
                timer1.Stop();
                textBox1.Text = "Worked in this session:\t" + wits.ToString();
                textBox2.Text = "Worked today:\t" + fii(true);
                textBox3.Text = "Worked total:\t" + fii(false);
                button3.Text = "Exit";
            }
            else
            {
                startd = DateTime.Now;
                working = true;
                dptime();
                timer1.Start();
                button1.Text = "End work";
                button3.Text = "Hide app";
                textBox3.Text = "Worked today:\t" + fii(true);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            dptime();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(working)
            {
                Hide();
                StillRunning.Visible = true;
            }
            else
                this.Close();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            StillRunning.Visible = false;
        }
    }
    
}
