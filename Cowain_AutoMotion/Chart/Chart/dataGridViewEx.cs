using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chart
{
    public partial class dataGridViewEx : DataGridView
    {
        public dataGridViewEx()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer |
               ControlStyles.OptimizedDoubleBuffer |
               ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();
        }
    }
}
