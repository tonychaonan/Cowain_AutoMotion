using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chart
{
    public partial class Form1 : Form
    {
        private Chartcapacity chartcapacity1;

        private ChartTime chartTime1;
        private ErrorUnit errorUnit1;
        //private Nordson232cs nordson232cs1;
      //  private LOCTITE232 LOCTITE2321;
     //   private NordsonModbus NordsonModbus1;
        private CTUnit ctunit1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           // NordsonModbus1 = new NordsonModbus("192.168.1.9");
            ctunit1 = new CTUnit(30,1);
               chartcapacity1 = new Chartcapacity(30);
            chartTime1 = new ChartTime(30);
            errorUnit1 = new ErrorUnit(30);
            //nordson232cs1 = new Nordson232cs("COM6");
          //  LOCTITE2321 = new LOCTITE232("COM6");
            chartcapacity1.Dock = DockStyle.Fill;
            chartTime1.Dock = DockStyle.Fill;
            errorUnit1.Dock = DockStyle.Fill;
            panel1.Controls.Add(chartcapacity1);
            panel2.Controls.Add(chartTime1);
            panel3.Controls.Add(errorUnit1);
            //panel4.Controls.Add(nordson232cs1.norDson232unit1);
           //  panel5.Controls.Add(LOCTITE2321.LOCTITE232unit1);
          //  panel6.Controls.Add(NordsonModbus1.NorDsonModbusunit1);
            panel7.Controls.Add(ctunit1);
            errorUnit1.StationID = "B1011";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stopwatch sw1 = new Stopwatch();
        
           sw1.Restart();
            chartcapacity1.AddOkLeft();

            sw1.Stop();
         
        }

        private void chartcapacity1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stopwatch sw1 = new Stopwatch();
           
            sw1.Restart();
          //  chartcapacity1.AddDTRight();

            sw1.Stop();
          

        }

        private void button4_Click(object sender, EventArgs e)
        {
            chartcapacity1.AddOkRight();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            chartcapacity1.AddDT();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            chartTime1.StartRun();
            chartTime1.Start();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            chartTime1.StartWait();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            chartTime1.StartError();
        }

        private void chartTime1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
            Application.ExitThread() ;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            errorUnit1.StartErrorMessage(textBox1.Text);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            errorUnit1.RestErrorMessage();
        }

        private void 添加操作_Click(object sender, EventArgs e)
        {
            errorUnit1.AddActionMessage(textBox1.Text);

        }

        private void button10_Click(object sender, EventArgs e)
        {
            errorUnit1.EndErrorMessage(textBox1.Text);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ctunit1.StartDoLeft(textBox2.Text);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ctunit1.EndDoLeft("");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            ctunit1.StartDoRight(textBox2.Text);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            ctunit1.EndDoRight("");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                chartTime1.StartError();
            }
            else
            {
                chartTime1.StartWait();
            }
        }
    }

  
}
