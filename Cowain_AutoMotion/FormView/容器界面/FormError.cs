using Chart;
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
    public partial class FormError : Form
    {
        public ErrorUnit ErrorUnit1;
        public FormError(int days)
        {
            InitializeComponent();
       
            ErrorUnit1 = new ErrorUnit(days);
            ErrorUnit1.Dock = DockStyle.Fill;
            panel1.Controls.Add(ErrorUnit1);
        }
    }
}
