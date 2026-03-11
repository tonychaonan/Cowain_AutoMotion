using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Excel;

namespace Cowain_AutoMotion.FormView
{
    public partial class frm_Help : Form
    {
        public frm_Help()
        {
            InitializeComponent();
        }

        private void frm_Help_Load(object sender, EventArgs e)
        {
            string file = Application.StartupPath + "\\Config\\ERROR.xls";
            ExcelHelper.Excel2Dgv(dataGridView1, file, "ERROR");
        }
    }
}
