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
using Cowain_Machine.Flow;
using Cowain_AutoMotion;
using Cowain_AutoMotion.Flow;

namespace Cowain_Form.FormView
{
    public partial class VisionForm : DevExpress.XtraEditors.XtraForm
    {
        public VisionForm()
        {
            InitializeComponent();
            Init();
        }

        #region 自定义变量
        public delegate void VisionDelegate(PictureInfo picInfo);
        public event VisionDelegate VisionEvent;
        /// <summary>
        /// 图片详细信息
        /// </summary>
        public class PictureInfo
        {
            /// <summary>
            /// 图片名
            /// </summary>
            public string PicName = string.Empty;
            /// <summary>
            /// 相机名
            /// </summary>
            public string CCDName = string.Empty;
            /// <summary>
            /// 单位
            /// </summary>
            public string Unit = string.Empty;
            /// <summary>
            /// 角度
            /// </summary>
            public decimal Angle = 0;
            /// <summary>
            /// 图片详情
            /// </summary>
            public string Detail = string.Empty;
        }
        //各工站图片信息
        private List<PictureInfo> m_picList = new List<PictureInfo>();

        MESLXData mESLXData = (MESLXData)MESDataDefine.MESLXData;
        #endregion

        #region 自定义方法
        /// <summary>
        /// 图片显示
        /// </summary>
        /// <param name="picName">图片名</param>
        /// <param name="picedit">显示图片的控件</param>
        private void ShowPic(string picName,PictureEdit picedit)
        {
            try
            {
                //string path = Path.Combine(mESLXData.StrLocalPath, picName);
                string path = picName;
                if (!File.Exists(path))
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
            catch (Exception)
            {
                
            }
            
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            m_picList.Clear();
            PictureInfo pic = new PictureInfo();
            for (int i=0;i<4; i++)
            {
                m_picList.Add(pic);
            }
        }
        #endregion

        private void picOpen_Click(object sender, EventArgs e)
        {

        }

        private void pictureEdit1_1_DoubleClick(object sender, EventArgs e)
        {
            PictureEdit picedit = (PictureEdit)sender;
            PictureInfo picInfo = new PictureInfo();
            if(picedit.Tag.ToString() == "0")
            {
                //1_1
                picInfo = m_picList[0];
            }
            else if (picedit.Tag.ToString() == "1")
            {
                //1_2
                picInfo = m_picList[1];
            }
            else if (picedit.Tag.ToString() == "2")
            {
                //2_1
                picInfo = m_picList[2];
            }
            else if (picedit.Tag.ToString() == "3")
            {
                //2_2
                picInfo = m_picList[3];
            }
            VisionEvent(picInfo);
        }
    }
}