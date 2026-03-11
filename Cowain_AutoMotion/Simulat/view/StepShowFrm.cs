using MotionBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain_AutoMotion.Simulat.view
{
    public partial class StepShowFrm : Form
    {
        Thread thread;
        private bool reStart = false;
        private string currentInstanceName = "";
        private bool b_Stop = false;
        /// <summary>
        /// 当前step
        /// </summary>
        string currentStepStr = "";
        public StepShowFrm()
        {
            InitializeComponent();
            thread = new Thread(showStep);
            thread.IsBackground = true;
            thread.Start();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            listViewStepInstanceName.Items.Clear();
            List<string> listNames = ShowStepManager.instance.getAllShowStepInstanceNames();
            foreach (var item in listNames)
            {
                listViewStepInstanceName.Items.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listViewStepInstanceName.SelectedItems.Count > 0)
            {
                label1.Text = listViewStepInstanceName.SelectedItems[0].Text.ToString();
                txtStep.Text = label1.Text;
                currentInstanceName = label1.Text;
                reStart = true;
                tabControl1.SelectedIndex = 0;
            }
        }
        private void showStep()
        {
            while (true)
            {
                try
                {
                    if (b_Stop)
                    {
                        break;
                    }
                    Thread.Sleep(100);
                    if (reStart)
                    {
                        reStart = false;
                        dataGridView1.Invoke(new Action(() =>
                        {
                            dataGridView1.Rows.Clear();
                        }));
                        ShowStep showStep = ShowStepManager.instance.getShowStep(currentInstanceName);
                        showStep.show = true;
                        if (showStep == null)
                        {
                            continue;
                        }
                        List<string> steps = showStep.getStepList();
                        foreach (var item in steps)
                        {
                            dataGridView1.Invoke(new Action(() =>
                            {
                                dataGridView1.Rows.Add(new string[] { item });
                            }));
                        }
                    }
                    ShowStep showStep1 = ShowStepManager.instance.getShowStep(currentInstanceName);
                    if (showStep1 == null)
                    {
                        continue;
                    }
                    string stepStr = showStep1.getcurrentStep();
                    if (stepStr == currentStepStr)
                    {
                        continue;
                    }
                    currentStepStr = stepStr;
                    dataGridView1.BeginInvoke(new Action(() =>
                    {
                        foreach (DataGridViewRow item in dataGridView1.Rows)
                        {
                            if (item.Cells[0].Style.BackColor == Color.Green)
                            {
                                item.Cells[0].Style.BackColor = Color.Gray;
                            }
                            if (item.Cells[0].Value.ToString() == stepStr)
                            {
                                item.Cells[0].Style.BackColor = Color.Green;
                            }
                        }
                    }));
                }
                catch { }
            }
        }

        private void StepShowFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            b_Stop = true;
        }
    }
}
