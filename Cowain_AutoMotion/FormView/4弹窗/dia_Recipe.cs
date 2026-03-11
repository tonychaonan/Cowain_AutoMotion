using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cowain_Form.FormView
{
   public partial class dia_Recipe : Form
   {
      public dia_Recipe()
      {
         InitializeComponent();
      }
      public string RecipeID = "";
      private void btn_Cancel_Click(object sender, EventArgs e)
      {
         RecipeID = "";
         this.Close();
      }

      private void btn_OK_Click(object sender, EventArgs e)
      {
         RecipeID = textBox1.Text.Trim().ToUpper();
         this.Close();
      }
   }
}
