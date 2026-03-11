using System;
using System.Windows.Forms;

namespace Cowain_AutoMotion.FormView
{
    public partial class frm_Set_Login : Form
    {
        private string _passWord;
        private string _errorMessage = "密码错误，请联系生计部门";
        public frm_Set_Login(String passWord, string errorMessage = "")
        {
            this._passWord = passWord;
            this._errorMessage = errorMessage == "" ? this._errorMessage : errorMessage;
            InitializeComponent();
        }
        
        private void btn_Check_Click(object sender, EventArgs e)
        {
            if (tbx_PassWord.Text == this._passWord)
            {
                this.DialogResult = DialogResult.OK;
                this.Dispose();
            }
            else
            {
                this.Dispose();
                MessageBox.Show(this._errorMessage);
            }
        }
    }
}
