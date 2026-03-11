using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cowain_AutoMotion
{
    public partial class frm_ButtonManage : Form
    {
        List<frm_ButtonManageItem> listForms=new List<frm_ButtonManageItem>();  
        List<string> keys=new List<string>();
        List<string> buttonTypes = new List<string>();
        public frm_ButtonManage()
        {
            InitializeComponent();
        }

        private void frm_ButtonManage_Load(object sender, EventArgs e)
        {
            //keys.AddRange(new string[] {"startButton", "pauseButton", "stopButton", "startButtonLED", "pauseButtonLED", "stopButtonLED",
            //    "safeLight", "safeDoor", "emgButton", "greenLED", "yellowLED", "redLED", "buzzerLED"});
            //buttonTypes.AddRange( Enum.GetNames(typeof(ButtonType)).ToList());
            //for (int i = 0; i < keys.Count; i++)
            //{
            //    ButtonType buttonType;
            //    Enum.TryParse(buttonTypes[i], out buttonType); 
            //    frm_ButtonManageItem frm_ButtonManageItem = new frm_ButtonManageItem(keys[i], buttonType);
            //    frm_ButtonManageItem.Location = new Point(12 + frm_ButtonManageItem.Width * i, 12);
            //    frm_ButtonManageItem.Parent = this;
            //    frm_ButtonManageItem.Show();
            //}
        }
    }
}
