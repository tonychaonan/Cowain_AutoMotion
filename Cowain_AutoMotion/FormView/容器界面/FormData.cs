using Chart;
using Cowain_Machine.Flow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain_AutoMotion.FormView
{
    public partial class FormData : Form
    {
        private DateTime dtold;
        private DateTime dtnow;
        public ListCT ListCT1;
        public ListCT ListCT2;
        public bool Isuse = true;

        private bool first = false;
        Dictionary<int, CTnumbers> CTnumbersDic1 = new Dictionary<int, CTnumbers>();
        Dictionary<int, CTnumbers> CTnumbersDic2 = new Dictionary<int, CTnumbers>();

        List<string> listct1 = new List<string>();
        List<string> listct2 = new List<string>();

        private int second = 600;
        //表示流程步数
        private int stepcount = 8;
        public Chartcapacity Chartcapacity1;

        public ChartTime ChartTime1;

        public CTUnit CTUnit1;
        public FormData(int days)
        {
            InitializeComponent();
            if (Chart.Common._ConfigDTSTEP == null)
                Chart.Common.GetStepData();
            //stepcount = 0;
            Chartcapacity1 = new Chartcapacity(days);
            ChartTime1 = new ChartTime(days);
            int gantryCount = 1;
            //CTUnit1 = new CTUnit(days, gantryCount);
            CTUnit1 = new CTUnit(days, gantryCount);
            Chartcapacity1.Dock = DockStyle.Fill;
            panel1.Controls.Add(Chartcapacity1);

            dtold = DateTime.Now;
            if (Chart.Common._ConfigDTSTEP == null)
                Chart.Common.GetStepData();
            //int num = Chart.Common._ConfigDTSTEP.Columns.Count;
            listct1 = new List<string>();
            listct2 = new List<string>();
            for (int i = 0; i < stepcount; i++)
            {
                CTnumbersDic1.Add(i + 1, new CTnumbers() { stepID = i });
                CTnumbersDic2.Add(i + 1, new CTnumbers() { stepID = stepcount + i + 1 });
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            Chartcapacity1.Dock = DockStyle.Fill;
            panel1.Controls.Add(Chartcapacity1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            ChartTime1.Dock = DockStyle.Fill;
            panel1.Controls.Add(ChartTime1);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            CTUnit1.Dock = DockStyle.Fill;
            panel1.Controls.Add(CTUnit1);
        }
        private void addcoloum(DataGridView dataGridView1)
        {
          
            if (Chart.Common._ConfigDTSTEP == null)
            {
                Chart.Common.GetStepData();
              
            }
            dataGridView1.Rows.Add("************");
        }
        bool locker = false;
        private void FormData_Load(object sender, EventArgs e)
        {
            locker = true;
            locker = false;
        }
    }

    public class CTnumbers
    {
        public int stepID = 0;
        public string step = "";
        public DateTime Dtend;
        public DateTime Dtstart;
        public int Start = 0;
        public int End = 0;
    }
}
