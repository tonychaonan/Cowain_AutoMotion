using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;
using Cowain_AutoMotion.Flow;
using Cowain_AutoMotion;

namespace Cowain_Form.FormView
{
    public partial class VisionDetail : DevExpress.XtraEditors.XtraForm
    {
        public VisionDetail()
        {
            InitializeComponent();
        }

        #region 自定义变量
        public VisionForm.PictureInfo m_PictureInfo = new VisionForm.PictureInfo();
        MESLXData mESLXData = (MESLXData)MESDataDefine.MESLXData;
        #endregion

        #region 自定义方法
        /// <summary>
        /// 显示信息
        /// </summary>
        private void DoRefresh()
        {
            #region 显示图片
            ShowPic(m_PictureInfo.PicName, pictureEdit);
            #endregion

            #region 显示图片信息
            memoInfo.Text = m_PictureInfo.Detail;
            #endregion
        }

        /// <summary>
        /// 图片显示
        /// </summary>
        /// <param name="picName">图片名</param>
        /// <param name="picedit">显示图片的控件</param>
        private void ShowPic(string picName, PictureEdit picedit)
        {
            string path = Path.Combine(mESLXData.StrCCDPicturePath, picName);
            if(!File.Exists(path))
            {
                return;
            }
            FileStream fs = new FileStream(path, FileMode.Open);
            byte[] picbytes = new byte[fs.Length];

            BinaryReader br = new BinaryReader(fs);
            picbytes = br.ReadBytes((int)fs.Length);

            fs.Close();
            fs.Dispose();

            MemoryStream ms = new MemoryStream(picbytes);
            Bitmap bmpt = new Bitmap(ms);

            picedit.Image = bmpt;
        }
        #endregion



        private void VisionDetail_Load(object sender, EventArgs e)
        {
            DoRefresh();
        }
    }
}