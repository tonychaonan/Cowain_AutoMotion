using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Chart
{
    public partial class ListCT : UserControl
    {
        
        ListViewItem li1;

        private DateTime dtold;
        private DateTime dtnow;
        string str;
        public ListCT(string str)
        {
            InitializeComponent();
            this.str = str;
        }

  
        private void ListCT_Load(object sender, EventArgs e)
        {
            if (label1.InvokeRequired)
            {
                label1.BeginInvoke(new Action(() =>
                {
                    label1.Text = str;
                }));
            }
            else
            {
                label1.Text = str;
            }
        }

        public void AddFirst(string message)
        {
            try
            {
                dtold = DateTime.Now;
                if (listView1.InvokeRequired)
                {
                    listView1.BeginInvoke(new Action(() =>
                    {
                        listView1.Items.Clear();
                        li1 = new ListViewItem(message);
                        li1.SubItems.Add("0");
                        listView1.Items.Add(li1);

                    }));
                }
                else
                {
                    listView1.Items.Clear();
                    li1 = new ListViewItem(message);
                    li1.SubItems.Add("0");
                    listView1.Items.Add(li1);
                }

            }
            catch 
            {

              
            }
            
   
        }
        public void Add(string message)
        {
            try
            {
                dtnow = DateTime.Now;
                TimeSpan ts = dtnow - dtold;
                if (listView1.InvokeRequired)
                {
                    listView1.BeginInvoke(new Action(() =>
                    {

                        li1 = new ListViewItem(message);
                        li1.SubItems.Add((ts.TotalMilliseconds / 1000.0).ToString("0.0"));
                        listView1.Items.Add(li1);
                        if (listView1.Items.Count > 40)
                            listView1.Items.Clear();

                    }));
                }
                else
                {
                    li1 = new ListViewItem(message);
                    li1.SubItems.Add((ts.TotalMilliseconds / 1000.0).ToString("0.0"));
                    listView1.Items.Add(li1);
                    if (listView1.Items.Count > 40)
                        listView1.Items.Clear();
                }
            }
            catch 
            {

                
            }
            
           
        }
    }
}
