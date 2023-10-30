using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        SerialPort sp = new SerialPort("COM3", 115200, Parity.None, 8, StopBits.One);
        string inStr = "";
        string[] ss = new string[20];

        int chart_cnt1 = 0;
        int chart_cnt2 = 0;



        public Form1()
        {
            InitializeComponent();
            sp.DataReceived += new SerialDataReceivedEventHandler(onReceive);
            sp.Open();
        }
        void onReceive(object sender, SerialDataReceivedEventArgs e)
        {
            inStr += sp.ReadExisting(); while ( (inStr.Length > 106) && (inStr[106] != 10) && (inStr[105] != 13) ) inStr = inStr.Substring(1);
            
            if  ((inStr.Length > 106) && (inStr[106] == 10) && (inStr[105] == 13) && (inStr[0] == 90)) 
            {
                    string tempString = inStr.Substring(1, 104);
                    inStr = inStr.Substring(106);

                //textBox1.Invoke(new Action(() => { textBox1.Text = tempString; }));

                

                    for (int i = 0; i < 16; i++) ss[i] = tempString.Substring(i * 6, 6);

                // order correction
                    string ts;
                for (int i = 0; i < 4; i++) 
                {
                    ts = ss[i + 8]; ss[i + 8] = ss[i + 0]; ss[i + 0] = ts; // ch 1
                    ts = ss[i + 12]; ss[i + 12] = ss[i + 4]; ss[i + 4] = ts; // ch 2
//                    ts = ss[i + 12]; ss[i + 12] = ss[i + 4]; ss[i + 4] = ts; // ch 2


                }




                    ss[16] = tempString.Substring(96, 8);

                int[] _temp = new int[17]; 
                try { for (int i = 0; i < 17; i++) _temp[i] = Convert.ToInt32(ss[i], 16) >> 4; } catch { return; };

                for (int i = 0; i < 16; i++) adcData.currentAdc[i] = _temp[i]; 
                adcData.currentTime_mc = _temp[16]; adcData.freshData(); 

               // textBox2.Invoke(new Action(() => { textBox2.Text = ""; for (int i = 0; i < 16; i++) textBox2.Text += adcData.currentAdc[i].ToString() +":";}));
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++) chart1.Series[i].Points.Clear();

            adcData.Zero();

            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //   textBox1.Invoke(new Action(() => { textBox1.Text = ""; for (int i = 0; i < 16; i++) textBox1.Text += adcData.zeroedAdc[i].ToString() + ":"; }));
            //  textBox2.Invoke(new Action(() => { textBox2.Text = ""; for (int i = 0; i < 16; i++) textBox2.Text += adcData.filteredAdc[i].ToString() + ":"; }));
            // for (int i = 12; i < 16; i++) chart1.Series[i].Points.AddY(adcData.filteredAdc[i]); //  adcData.middledAdc[i] - adcData.zeroAdc[i]

            chart1.Series[0].Points.AddY(adcData.weights[0]);
            chart1.Series[1].Points.AddY(adcData.weights[1]);
            chart1.Series[2].Points.AddY(adcData.weights[2]);
            chart1.Series[3].Points.AddY(adcData.weights[3]);

            //chart1.Series[1].Points.AddY(adcData.diffX[3]);
            //chart1.Series[2].Points.AddY(adcData.diffY[3]);

        }
    }
}
