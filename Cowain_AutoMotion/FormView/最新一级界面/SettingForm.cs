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
using Cowain_Machine.Flow;
using Cowain_AutoMotion;
using Cowain;
using ReadAndWriteConfig;
using Cowain_AutoMotion.Flow;
using Cowain_AutoMotion.FormView._4弹窗;
using DevExpress.XtraEditors.Controls;
using static Cowain_AutoMotion.SQLSugarHelper;
using System.Reflection;
using MotionBase;
using Chart;
using System.Linq.Expressions;
using Cowain_Machine;
using Cowain_AutoDispenser.FormView._4弹窗;

namespace Cowain_Form.FormView
{


    public partial class SettingForm : DevExpress.XtraEditors.XtraForm
    {
        public SettingForm(clsMachine pM)
        {
            InitializeComponent();
            pMachine = pM;
            LoadForm();
        }

        #region 自定义变量

        public clsMachine pMachine;

        /// <summary>
        /// 增加或修改弹窗字段
        /// </summary>
        UserInformationForm userInformationForm;

        /// <summary>
        /// 身份信息
        /// </summary>
        public enum Identity
        {
            製造商,
            設備工程師,
            製程工程師,
            工程師,
            生產員,
        }


        DataTable m_ParamTable = new DataTable();
        public enum enformList
        {
            enSetting = 0,
            enUser,
            enHomeForm,
            enAutoForm,
            enRecipeForm,
            enTeachForm,
            enManulForm,
            enConveyorForm,
            enErrorForm,
            enDataForm,
            enMax,
        };

        private Form[] pFormView = new Form[(int)(enformList.enMax)];

        #endregion

        #region 自定义方法
        /// <summary>
        /// 读取相关参数
        /// </summary>
        private void ReadParam()
        {
            if (MachineDataDefine.MachineCfgS == null)
            {
                MsgBoxHelper.DxMsgShowErr("读取配置信息失败！");
                return;
            }

            cmb_station.Properties.Items.Clear();

            foreach (var item in MachineDataDefine.settingData.GeneralConfig.ProdType)
            {
                cmb_Style.Properties.Items.Add(item);
            }
            cmb_Style.Text = MachineDataDefine.settingData.ProdType;
            //foreach (var item in MachineDataDefine.settingData.GeneralConfig.Line)
            //{
            //    cmb_line.Properties.Items.Add(item);
            //}
            cmb_line.Text = MachineDataDefine.settingData.Line;
            foreach (var item in MachineDataDefine.settingData.GeneralConfig.Station)
            {
                cmb_station.Properties.Items.Add(item);
            }
            cmb_station.Text = MachineDataDefine.settingData.Station;
            foreach (var item in MachineDataDefine.settingData.GeneralConfig.Site)
            {
                cmb_site.Properties.Items.Add(item);
            }
            cmb_site.Text = MachineDataDefine.settingData.Site;
            foreach (var item in MachineDataDefine.settingData.GeneralConfig.Machine)
            {
                spinEdit_qpl.Properties.Items.Add(item);
            }
            spinEdit_qpl.Text = MachineDataDefine.settingData.Machine;
            ////读取语言
            string lan = ConfigHelper.GetAppConfig("Paramter", "Language");
            foreach (var item in MachineDataDefine.settingData.GeneralConfig.Language)
            {
                cmb_language.Properties.Items.Add(item);
            }

            cmb_language.SelectedIndex = ConvertHelper.GetDef_Int(lan, 1) - 1;
            //#region Information


        }

        private void LoadForm()
        {
            //回原
            frm_Home fmHome = new frm_Home(ref pMachine);
            fmHome.TopLevel = false;
            fmHome.Dock = DockStyle.Fill;
            fmHome.FormBorderStyle = FormBorderStyle.None;
            fmHome.Show();
            TabPage回原.Controls.Add(fmHome);
            pFormView[(int)enformList.enHomeForm] = fmHome;
            //自动
            frm_Auto fmAuto = new frm_Auto(ref pMachine);
            fmAuto.TopLevel = false;
            fmAuto.Dock = DockStyle.Fill;
            fmAuto.FormBorderStyle = FormBorderStyle.None;
            //fmAuto.Show();
            TabPage自动.Controls.Add(fmAuto);
            pFormView[(int)enformList.enAutoForm] = fmAuto;
            //胶路参数
            frm_Recipe fmRecipe = new frm_Recipe(ref pMachine);
            fmRecipe.TopLevel = false;
            fmRecipe.Dock = DockStyle.Fill;
            fmRecipe.FormBorderStyle = FormBorderStyle.None;
            //fmRecipe.Show();
            TabPage胶路参数.Controls.Add(fmRecipe);
            pFormView[(int)enformList.enRecipeForm] = fmRecipe;
            //设备参数
            frm_Teach fmTeach = new frm_Teach(ref pMachine);
            fmTeach.TopLevel = false;
            fmTeach.Dock = DockStyle.Fill;
            fmTeach.FormBorderStyle = FormBorderStyle.None;
            //fmTeach.Show();
            TabPage设备参数.Controls.Add(fmTeach);
            pFormView[(int)enformList.enTeachForm] = fmTeach;
            //手动
            frm_Manul fmManul = new frm_Manul(ref pMachine);
            fmManul.TopLevel = false;
            fmManul.Dock = DockStyle.Fill;
            fmManul.FormBorderStyle = FormBorderStyle.None;
            //fmManul.Show();
            TabPage手动.Controls.Add(fmManul);
            pFormView[(int)enformList.enManulForm] = fmManul;

            //报警
            frm_Main.formError.TopLevel = false;
            frm_Main.formError.Dock = DockStyle.Fill;
            frm_Main.formError.FormBorderStyle = FormBorderStyle.None;
            TabPage报警.Controls.Add(frm_Main.formError);
            pFormView[(int)enformList.enErrorForm] = frm_Main.formError;
            //运行数据
            frm_Main.formData.TopLevel = false;
            frm_Main.formData.Dock = DockStyle.Fill;
            frm_Main.formData.FormBorderStyle = FormBorderStyle.None;
            TabPage运行数据.Controls.Add(frm_Main.formData);
            pFormView[(int)enformList.enDataForm] = frm_Main.formData;
        }

        private void ShowUserButton(Sys_Define.enPasswordType enLoginType)
        {
            if (enLoginType == Sys_Define.enPasswordType.Operator ||
                enLoginType == Sys_Define.enPasswordType.UnLogin)
            {
                this.TabPage回原.PageVisible = true;
                this.TabPage自动.PageVisible = true;
                this.TabPage胶路参数.PageVisible = false;
                this.TabPage设备参数.PageVisible = false;
                this.TabPage手动.PageVisible = false;
                this.TabPage外部流道.PageVisible = true;
                this.TabPage报警.PageVisible = true;
                this.TabPage运行数据.PageVisible = true;
            }
            else if (enLoginType == Sys_Define.enPasswordType.Eng)
            {
                this.TabPage回原.PageVisible = true;
                this.TabPage自动.PageVisible = true;
                this.TabPage胶路参数.PageVisible = true;
                this.TabPage设备参数.PageVisible = true;
                this.TabPage手动.PageVisible = true;
                this.TabPage外部流道.PageVisible = true;
                this.TabPage报警.PageVisible = true;
                this.TabPage运行数据.PageVisible = true;
            }
            else if (enLoginType == Sys_Define.enPasswordType.MacEng)  //enLoginType == Sys_Define.enPasswordType.ItEng ||
            {
                this.TabPage回原.PageVisible = true;
                this.TabPage自动.PageVisible = true;
                this.TabPage胶路参数.PageVisible = true;
                this.TabPage设备参数.PageVisible = true;
                this.TabPage手动.PageVisible = false;
                this.TabPage外部流道.PageVisible = true;
                this.TabPage报警.PageVisible = true;
                this.TabPage运行数据.PageVisible = true;
            }
            else if (enLoginType == Sys_Define.enPasswordType.Maker)
            {
                this.TabPage回原.PageVisible = true;
                this.TabPage自动.PageVisible = true;
                this.TabPage胶路参数.PageVisible = true;
                this.TabPage设备参数.PageVisible = true;
                this.TabPage手动.PageVisible = true;
                this.TabPage外部流道.PageVisible = true;
                this.TabPage报警.PageVisible = true;
                this.TabPage运行数据.PageVisible = true;
            }
        }

        public void SetUserButton(Sys_Define.enPasswordType enLoginType)
        {
            ShowUserButton(enLoginType);
        }
        #endregion
        public static List<Control> control = new List<Control>(); //Login中所有控件
        private void SettingForm_Load(object sender, EventArgs e)
        {
            ReadParam();


            ComboBoxItemCollection cNameCollection = cmb_cName.Properties.Items;
            ComboBoxItemCollection userNameCollection = cmb_userName.Properties.Items;
            var listMotors = DBContext<PWD>.GetInstance().GetList(); //获取当前的数据表
            cNameCollection.Add("All");
            userNameCollection.Add("All");
            //  getControl(this.Controls);
            int Widths = 0, Heights = 0, Widths2 = 0, Heights2 = 0;
            control.Clear();
            getControl(this.Controls);
            for (int i = 0; i < control.Count; i++)
            {
                if (control[i] is Label)
                {
                    Widths = control[i].Width;
                    Heights = control[i].Height;
                    ((Label)control[i]).AutoEllipsis = true;
                }
                if (control[i] is CheckBox)
                {
                    Widths2 = control[i].Width;
                    Heights2 = control[i].Height;
                    ((CheckBox)control[i]).AutoEllipsis = true;
                }
                // }
                //将所有控件txt变成可转语言
                control[i].Text = JudgeLanguage.JudgeLag(control[i].Text);
                if (control[i] is Label)
                {
                    control[i].Width = Widths;
                    control[i].Height = Heights;
                    ((Label)control[i]).AutoSize = false;
                    ((Label)control[i]).AutoEllipsis = true;
                }
                if (control[i] is CheckBox)
                {
                    control[i].Width = Widths2;
                    control[i].Height = Heights2;
                    ((CheckBox)control[i]).AutoSize = false;
                    ((CheckBox)control[i]).AutoEllipsis = true;
                }
                else if (control[i] is Button)
                {
                    ((Button)control[i]).AutoEllipsis = true;
                }

            }
            for (int i = 0; i < MachineDataDefine.controlSettingForm.Count; i++)
            {
                MachineDataDefine.controlSettingForm[i].Text = JudgeLanguage.JudgeLag(MachineDataDefine.controlSettingForm[i].Text);
            }
            //添加所有用户信息
            foreach (var item in listMotors)
            {
                userNameCollection.Add(item.UserName);
            }

            //添加身份信息
            List<string> identityList = Enum.GetNames(typeof(Identity)).ToList();
            for (int i = 0; i < identityList.Count; i++)
            {
                cNameCollection.Add(identityList[i]);
            }

            //显示combox中信息
            cmb_cName.SelectedIndex = 0;
            cmb_userName.SelectedIndex = 0;
        }


        private void SettingForm_Shown(object sender, EventArgs e)
        {
            //设置tbleLayout的分割线高
            this.tableLayoutPanel_panelZone.RowStyles[0].Height = 60;
            this.tableLayoutPanel_panelZone.RowStyles[1].Height = 50;

            this.gridControl.Enabled = true;
            this.gridView.OptionsBehavior.Editable = false;

            //设置某列不可以编辑
            //int index = 0;
            //foreach (var item in gridView.Columns)
            //{
            //    if (gridView.Columns[index].FieldName == "UserName")
            //    {
            //        gridView.Columns[index].OptionsColumn.AllowEdit = false;
            //    }
            //    index++;
            //}
        }

        private void xtraTabControlMain_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            int selectIndex = xtraTabControlMain.SelectedTabPageIndex;
            if (selectIndex == 0)
            {
                //labShow.Text = "Config Hard Ware " + MachineDataDefine.HardwareCfg.AxisTypeEnum.ToString()+@"\r\n123";
                if (pMachine.GetisAutoing())
                    panel_configContain.Enabled = false;
                else
                    panel_configContain.Enabled = true;
            }
            else if (selectIndex == 1)
            {
                if (pMachine.GetisAutoing())
                    tableLayoutPanel_panelZone.Enabled = false;
                else
                    tableLayoutPanel_panelZone.Enabled = true;
            }
            if (selectIndex < 2)
                return;
            if (pFormView[selectIndex] == null)
            {
                return;
            }
            if (pMachine.GetisAutoing())
            {
                pFormView[selectIndex].Enabled = false;
                //foreach (Control control in pFormView[selectIndex].Controls)
                //{
                //    if (control is TabControl)
                //    {
                //        int iu = 0;
                //    }
                //    else
                //    {
                //        control.Enabled = false;
                //    }
                //}
            }
            else
            {
                if (pFormView[selectIndex] == null)
                {
                    return;
                }
                pFormView[selectIndex].Enabled = true;
                //foreach (Control control in pFormView[selectIndex].Controls)
                //{
                //    if (control is TabControl)
                //    {
                //        int i = 0;
                //    }
                //    else
                //    {
                //        control.Enabled = true;
                //    }
                //}
            }

            for (int i = 0; i < (int)enformList.enMax; i++)
            {
                if (pFormView[i] != null)
                {
                    pFormView[i].Hide();
                }
            }
            pFormView[selectIndex].Show();
        }

        #region 用户管理

        /// <summary>
        /// 查询按钮click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_searchInfor_Click(object sender, EventArgs e)
        {
            if (pMachine.m_LoginUser != MotionBase.Sys_Define.enPasswordType.Maker)
            {
                MsgBoxHelper.DxMsgShowErr("当前登录用户权限不够!");
                return;
            }
            try
            {
                string selectedConditionStr = cmb_cName.SelectedItem.ToString();  //根据职业选择
                var listMotors = DBContext<PWD>.GetInstance().GetList(); //获取当前的数据表
                ClearAllRows();
                DataTable dt = null;
                if (selectedConditionStr == "All")
                {
                    dt = ListToTable(listMotors);
                    this.gridControl.DataSource = listMotors;

                }
                else
                {
                    List<PWD> oneRowInfor = new List<PWD>();
                    foreach (var item in listMotors)
                    {
                        if (item.CName.Trim() == selectedConditionStr)
                        {
                            oneRowInfor.Add(item);
                        }
                    }
                    dt = ListToTable(oneRowInfor);
                }
                dt.Columns.Add("Level", typeof(string));
                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    dt.Rows[i]["Level"] = GetLevel((string)dt.Rows[i]["EName"]);
                }

                gridControl.DataSource = dt;
                //this.gridView.PopulateColumns();
            }
            catch (Exception ex)
            {
                MsgBoxHelper.DxMsgShowErr("数据库查询时出错！" + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// 清除gridControl中所有行
        /// </summary>
        private void ClearAllRows()
        {
            bool _mutilSelected = gridView.OptionsSelection.MultiSelect;//获取当前是否可以多选
            if (!_mutilSelected)
                gridView.OptionsSelection.MultiSelect = true;
            gridView.SelectAll();
            gridView.DeleteSelectedRows();
            gridView.OptionsSelection.MultiSelect = _mutilSelected;//还原之前是否可以多选状态
        }

        /// <summary>
        /// list泛型转dataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public DataTable ListToTable<T>(IList<T> list) where T : class, new()
        {
            if (list == null) return null;
            Type type = typeof(T);
            DataTable dt = new DataTable();

            PropertyInfo[] properties = Array.FindAll(type.GetProperties(), p => p.CanRead);//判断此属性是否有Getter
            Array.ForEach(properties, prop => { dt.Columns.Add(prop.Name, prop.PropertyType); });//添加到列
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                Array.ForEach(properties, prop =>
                {
                    if (prop.PropertyType.FullName == "System.String")
                    {
                        row[prop.Name] = ConvertHelper.GetDef_Str(prop.GetValue(t, null)).Trim();

                    }
                    else
                    {
                        row[prop.Name] = prop.GetValue(t, null);
                    }

                });//添加到行
                dt.Rows.Add(row);
            }
            return dt;
        }

        /// <summary>
        /// dataTable转lsit泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public IList<T> TableToList<T>(DataTable dt) where T : class, new()
        {

            IList<T> ts = new List<T>();// 定义集合
            Type type = typeof(T);// 获得此模型的类型
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;
                    if (dt.Columns.Contains(tempName))// 检查DataTable是否包含此列
                    {
                        if (!pi.CanWrite) continue;// 判断此属性是否有Setter

                        object value = dr[tempName];
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }

            return ts;

        }


        /// <summary>
        /// 增加按钮click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_addNewInfor_Click(object sender, EventArgs e)
        {
            if (pMachine.m_LoginUser != MotionBase.Sys_Define.enPasswordType.Maker)
            {
                MsgBoxHelper.DxMsgShowInfo("当前登录用户权限不够!");
                return;
            }
            try
            {
                List<PWD> pDList = new List<PWD>();
                PWD pd = new PWD();
                userInformationForm = new UserInformationForm(pd, true);
                userInformationForm.ShowDialog();
                pd = userInformationForm.myPWD;
                if (pd != null)
                {
                    //获取表中最大的索引值
                    var listMotors = DBContext<PWD>.GetInstance().GetList(); //获取当前的数据表
                    int count = listMotors.Count;
                    int indexValue = 0;
                    if (count > 0)
                    {
                        if (count > 1)
                        {
                            indexValue = listMotors[0].ID;
                            for (int index = 1; index < count; ++index)
                            {
                                if (listMotors[index].ID > indexValue)
                                {
                                    indexValue = listMotors[index].ID;
                                }
                            }
                        }
                        else  //只有一项的时候
                        {
                            indexValue = listMotors[0].ID;
                        }
                    }
                    pd.ID = indexValue + 1;
                    AddConfigToDataTable(pd);
                    cmb_userName.Properties.Items.Add(pd.UserName);
                    //this.gridView.PopulateColumns();
                    pDList.Add(pd);
                    DBContext<PWD>.GetInstance().Insert(pDList);
                }

            }
            catch (Exception ex)
            {
                MsgBoxHelper.DxMsgShowErr("数据库添加时出错！" + ex.Message);
                //throw;
            }
        }

        /// <summary>
        /// 添加新的用户消息并且显示到gridView
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="list"></param>
        private void AddConfigToDataTable(PWD list)
        {
            DataTable dt;
            if (gridView.DataSource != null)
            {
                dt = ((DataView)gridView.DataSource).ToTable();
            }
            else
            {
                dt = new DataTable();
                dt.Columns.Add("ID", Type.GetType("System.Int32"));
                dt.Columns.Add("CName", Type.GetType("System.String"));
                dt.Columns.Add("EName", Type.GetType("System.String"));
                dt.Columns.Add("UserName", Type.GetType("System.String"));
                dt.Columns.Add("thePassWord", Type.GetType("System.String"));
                dt.Columns.Add("UserID", Type.GetType("System.String"));
                dt.Columns.Add("CardID", Type.GetType("System.String"));
                dt.Columns.Add("Level", Type.GetType("System.String"));

            }
            DataRow dataRow = dt.NewRow();
            dataRow["ID"] = list.ID;
            dataRow["CName"] = list.CName;
            dataRow["EName"] = list.EName;
            dataRow["UserName"] = list.UserName;
            dataRow["thePassWord"] = list.thePassWord;
            dataRow["UserID"] = list.UserID;
            dataRow["CardID"] = list.CardID;
            dataRow["Level"] = GetLevel(list.EName);
            dt.Rows.Add(dataRow);

            gridControl.DataSource = dt;
        }

        /// <summary>
        /// 编辑按钮click_事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_editInfor_Click(object sender, EventArgs e)
        {
            if (pMachine.m_LoginUser != MotionBase.Sys_Define.enPasswordType.Maker)
            {
                MsgBoxHelper.DxMsgShowInfo("当前登录用户权限不够!");
                return;
            }
            try
            {
                int rowHandle = gridView.FocusedRowHandle;
                PWD pd = new PWD();
                DataRow dataRow;
                if (rowHandle >= 0)
                {

                    dataRow = gridView.GetDataRow(rowHandle);
                    if (dataRow == null)
                        return;
                    pd.ID = ConvertHelper.GetDef_Int(dataRow["ID"]);
                    pd.CName = ConvertHelper.GetDef_Str(dataRow["CName"]).Trim();
                    pd.EName = ConvertHelper.GetDef_Str(dataRow["EName"]).Trim();
                    pd.UserName = ConvertHelper.GetDef_Str(dataRow["UserName"]).Trim();
                    pd.thePassWord = ConvertHelper.GetDef_Str(dataRow["thePassWord"]).Trim();
                    pd.CardID = ConvertHelper.GetDef_Str(dataRow["CardID"]).Trim();
                    pd.UserID = ConvertHelper.GetDef_Str(dataRow["UserID"]).Trim();
                }
                else
                {
                    MsgBoxHelper.DxMsgShowErr("请先选择要编辑的行");
                    return;
                }
                userInformationForm = new UserInformationForm(pd, false);
                userInformationForm.ShowDialog();
                pd = userInformationForm.myPWD;

                if (pd != null)
                {
                    if (DBContext<PWD>.GetInstance().Update(pd))
                    {
                        dataRow["ID"] = pd.ID;
                        dataRow["CName"] = pd.CName;
                        dataRow["EName"] = pd.EName;
                        dataRow["UserName"] = pd.UserName;
                        dataRow["thePassWord"] = pd.thePassWord;
                        dataRow["CardID"] = pd.CardID;
                        dataRow["UserID"] = pd.UserID;
                        dataRow["Level"] = GetLevel(pd.EName); ;
                    }
                    else
                    {
                        MsgBoxHelper.DxMsgShowErr("数据更新失败,请再次尝试!");
                    }
                }

                //if (btn_editInfor.Text=="编辑")
                //{
                //    btn_editInfor.Text = "保存";

                //}
                //else
                //{
                //    //检查单元格是否有空，且是否继续保存数据
                //    if (!CheckCellValueIsNull())
                //    {
                //        return;
                //    }

                //    if (SaveOrUpdataConfig())
                //    {
                //        btn_editInfor.Text = "编辑";
                //    }
                //    else
                //    {
                //        MsgBoxHelper.DxMsgShowErr("数据更新失败,请再次尝试!");
                //    }
                //}
            }
            catch (Exception ex)
            {
                MsgBoxHelper.DxMsgShowErr("数据库保存时出错！" + ex.Message);
            }
        }

        /// <summary>
        /// 检查是否单元格为空，且是否继续保存数据
        /// </summary>
        /// <returns></returns>
        private bool CheckCellValueIsNull()
        {
            bool isContinueSaveData = true;
            bool isBreakOuter = false;

            //检查每一行数据内容是否为空
            for (int i = 0; i < gridView.RowCount; i++)
            {
                DataRow row = gridView.GetDataRow(i); ;
                for (int j = 0; j < gridView.Columns.Count; j++)
                {
                    //从绑定的行数据直接取数据
                    string uidItem = (string)row[j];
                    if (uidItem == null || uidItem == String.Empty)
                    {
                        string msg = "有数据为null,是否还继续保存";
                        if (MessageBox.Show(msg + "?", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            isContinueSaveData = true;
                            isBreakOuter = true;
                            break;
                        }
                        else
                        {
                            isContinueSaveData = false;
                            isBreakOuter = true;
                            break;
                        }
                    }
                }
                if (isBreakOuter)
                {
                    break;
                }
            }
            return isContinueSaveData;
        }

        /// <summary>
        ///  保存或者更新当前data中数据
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="list"></param>
        /// <param name="index"></param>
        private bool SaveOrUpdataConfig()
        {
            bool result = false;
            //将每一行数据内容放置与list<>中
            var listMotors = DBContext<PWD>.GetInstance().GetList();
            for (int i = 0; i < gridView.RowCount; i++)
            {
                //获取当前行信息
                DataRow dataRow = gridView.GetDataRow(i);
                int isExitNum = 0;
                foreach (var item in listMotors)
                {
                    if ((string)dataRow["UserName"] == item.UserName)
                    {
                        item.ID = (int)dataRow["ID"];
                        item.CName = (string)dataRow["CName"];
                        item.EName = (string)dataRow["EName"];
                        item.UserName = (string)dataRow["UserName"];
                        item.thePassWord = (string)dataRow["thePassWord"];
                        isExitNum++;
                    }

                }
                if (isExitNum == 0)  //说明没有与之一样的
                {
                    PWD pWD = new PWD();
                    pWD.ID = (int)dataRow["ID"];
                    pWD.CName = (string)dataRow["CName"];
                    pWD.EName = (string)dataRow["EName"];
                    pWD.UserName = (string)dataRow["UserName"];
                    pWD.thePassWord = (string)dataRow["thePassWord"];
                    DBContext<PWD>.GetInstance().Insert(pWD);
                }
            }

            //批量更新数据库
            if (DBContext<PWD>.GetInstance().Update(listMotors))
            {
                result = true;
            }
            return result;
        }

        #endregion

        #region 信息配置
        /// <summary>
        /// 保存general信息按钮click事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_saveGeneral_Click(object sender, EventArgs e)
        {
            //if (MsgBoxHelper.DxMsgShowQues("确定要保存数据？") == DialogResult.Yes)
            //{
            #region 三国语言
            if (cmb_language.Text == "简体中文")
            {
                if (MachineDataDefine.LanguageStyle != 1)
                {
                    for (int i = 0; i < MachineDataDefine.FormVisibled.Length; i++)
                        MachineDataDefine.FormVisibled[i] = false;
                    MachineDataDefine.LanguageStyle = 1;
                    GetLanguage.LanguageNumber = "1";


                }
                ConfigHelper.UpdateAppConfig("Paramter", "Language", "1");
            }
            else if (cmb_language.Text == "English")
            {
                if (MachineDataDefine.LanguageStyle != 2)
                {
                    for (int i = 0; i < MachineDataDefine.FormVisibled.Length; i++)
                        MachineDataDefine.FormVisibled[i] = false;
                    MachineDataDefine.LanguageStyle = 2;
                    GetLanguage.LanguageNumber = "2";
                }
                ConfigHelper.UpdateAppConfig("Paramter", "Language", "2");
            }
            else
            {
                if (MachineDataDefine.LanguageStyle != 3)
                {
                    for (int i = 0; i < MachineDataDefine.FormVisibled.Length; i++)
                        MachineDataDefine.FormVisibled[i] = false;
                    MachineDataDefine.LanguageStyle = 3;
                    GetLanguage.LanguageNumber = "3";
                }
                ConfigHelper.UpdateAppConfig("Paramter", "Language", "3");
            }
            getControl(this.Controls);
            for (int i = 0; i < MachineDataDefine.controlSettingForm.Count; i++)
            {
                MachineDataDefine.controlSettingForm[i].Text = JudgeLanguage.JudgeLag(MachineDataDefine.controlSettingForm[i].Text);
            }
            MessageBox.Show(JudgeLanguage.JudgeLag("切换语言成功"));
            #endregion
            MachineDataDefine.settingData.Site = cmb_site.SelectedItem.ToString();
            //MachineDataDefine.MachineCfgS.Line = cmb_line.SelectedItem.ToString();
            MachineDataDefine.settingData.Line = cmb_line.Text.ToString();
            MachineDataDefine.settingData.ProdType = cmb_Style.SelectedItem.ToString();
            //MachineDataDefine.MachineCfgS.StrStationName = cmb_station.SelectedItem.ToString();
            MachineDataDefine.settingData.Station = cmb_station.SelectedItem.ToString();
            //MachineDataDefine.MachineCfgS.QPL = ConvertHelper.GetDef_Int(spinEdit_qpl.EditValue);
            MachineDataDefine.settingData.Machine = spinEdit_qpl.SelectedItem.ToString();
            MachineDataDefine.settingData.Language = cmb_language.SelectedItem.ToString();
            MachineDataDefine.MachineCfgS.WriteParams(MachineDataDefine.MachineCfgS);   //保存到这个位置
            if (MachineDataDefine.settingData.ProdType == "E_SKU")
            {
                MachineDataDefine.HardwareCfg.MaterialTypeEnum = MaterialType.E_SKU;
            }
            else if (MachineDataDefine.settingData.ProdType == "M_SKU")
            {
                MachineDataDefine.HardwareCfg.MaterialTypeEnum = MaterialType.M_SKU;
            }
            else if (MachineDataDefine.settingData.ProdType == "Normal")
            {
                MachineDataDefine.HardwareCfg.MaterialTypeEnum = MaterialType.Normal;
            }
            else
            {
                MachineDataDefine.HardwareCfg.MaterialTypeEnum = MaterialType.Common;
            }
            int index = cmb_language.SelectedIndex;
            ConfigHelper.UpdateAppConfig("Paramter", "Language", (index + 1).ToString());
            MachineDataDefine.settingData.WriteParams(MachineDataDefine.settingData);
            //MsgBoxHelper.DxMsgShowInfo("数据保存成功！");
            //}
        }
        #endregion

        private void btn_export_Click(object sender, EventArgs e)
        {
            if (gridControl.DataSource == null)
            {
                return;
            }
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.InitialDirectory = @"D:\";//打开初始目录
            saveFile.Title = "选择保存文件";
            saveFile.Filter = "csv files (*.csv)|*.csv|All files (*.*)|";//过滤条件
            saveFile.FilterIndex = 1;//获取第二个过滤条件开始的文件拓展名
            saveFile.FileName = "";//默认保存名称
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                DataTable sumTable = (DataTable)gridControl.DataSource;
                for (int i = 0; i < sumTable.Rows.Count; i++)
                {
                    sumTable.Rows[i][4] = "";
                }
                CSVFileHelper.SaveCSV(sumTable, saveFile.FileName);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (pMachine.m_LoginUser != MotionBase.Sys_Define.enPasswordType.Maker)
            {
                MsgBoxHelper.DxMsgShowInfo("当前登录用户权限不够!");
                return;
            }
            try
            {
                if (MsgBoxHelper.DxMsgShowQues("是否确认删除?") == DialogResult.Yes)
                {
                    int rowHandle = gridView.FocusedRowHandle;
                    if (rowHandle >= 0)
                    {

                        DataRow dataRow = gridView.GetDataRow(rowHandle);
                        PWD pwd = new PWD();
                        pwd.ID = ConvertHelper.GetDef_Int(dataRow["ID"]);
                        DBContext<PWD>.GetInstance().Delete(pwd);

                        gridView.GetDataRow(rowHandle).Delete();
                    }
                    else
                    {
                        MsgBoxHelper.DxMsgShowErr("请先选择要删除的行");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBoxHelper.DxMsgShowErr("数据库删除时出错！" + ex.StackTrace);
            }
        }

        public string GetLevel(string str)
        {
            string re;
            if (str.Contains("Maker"))
            {
                re = "Level3";
            }
            else if (str.Contains("Eng"))
            {
                re = "Level2";
            }
            else
            {
                re = "Level1";
            }
            return re;
        }

        private void btnAsync_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            //SaveFileDialog saveFile = new SaveFileDialog();
            openFile.InitialDirectory = @"D:\";//打开初始目录
            openFile.Title = "选择保存文件";
            openFile.Filter = "mdb files (*.mdb)|*.mdb|All files (*.*)|";//过滤条件
            openFile.FilterIndex = 1;//获取第二个过滤条件开始的文件拓展名
            openFile.FileName = "";//默认保存名称
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                //读取目标文件用户信息
                var listMotors = DBContext<PWD>.GetInstance(openFile.FileName).GetList();

                if (listMotors.Count > 0)
                {
                    Expression<Func<PWD, bool>> cd = null;
                    bool result = DBContext<PWD>.GetInstance().Delete(cd = f => f.ID > 0);

                    //开始插入数据
                    foreach (PWD item in listMotors)
                    {
                        //if (item.CName.Trim() == selectedConditionStr)
                        //{
                        //    oneRowInfor.Add(item);
                        //}
                        if (item.CardID == null)
                        {
                            item.CardID = "";
                        }
                        if (item.UserID == null)
                        {
                            item.UserID = "";
                        }
                        DBContext<PWD>.GetInstance().Insert(item);
                    }
                }
            }
        }

        private void btn_Config_Click(object sender, EventArgs e)
        {
            //if (MachineDataDefine.machineState.b_UseRemoteQualification)
            //{
            //    btn_Config.ForeColor= Color.Red;
            //    MachineDataDefine.machineState.b_UseRemoteQualification = false;
            //}
            //else
            //{
            //    btn_Config.ForeColor = Color.Lime;
            //    MachineDataDefine.machineState.b_UseRemoteQualification = true;
            //}
            frm_HardWare frm_HardWare = new frm_HardWare();
            if (frm_HardWare.ShowDialog() == DialogResult.OK)
            {

            }
        }

        private void cmb_language_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_language.SelectedIndex == 0)
            {
                lab语言.Text = "语言:";
                labSite.Text = "地点:";
                labLine.Text = "线号:";
                labStation.Text = "站别:";
                labMachine.Text = "设备号#:";
            }
            else if (cmb_language.SelectedIndex == 1)
            {
                lab语言.Text = "Language:";
                labSite.Text = "Site:";
                labLine.Text = "Line:";
                labStation.Text = "Station:";
                labMachine.Text = "Machine#:";
            }
            else
            {
                lab语言.Text = "Ngôn ngữ:";
                labSite.Text = "Địa điểm:";
                labLine.Text = "Đường dây số:";
                labStation.Text = "Đứng yên:";
                labMachine.Text = "Thiết bị #:";
            }
        }

        

        private void btn_Config_MouseDown(object sender, MouseEventArgs e)
        {
            // MachineDataDefine.machineState.b_UseRemoteQualification = true;
        }

        private void btn_Config_MouseUp(object sender, MouseEventArgs e)
        {
            //MachineDataDefine.machineState.b_UseRemoteQualification = false;
        }
        private void getControl(Control.ControlCollection etc)
        {
            foreach (Control ct in etc)
            {
                try
                {
                    MachineDataDefine.controlSettingForm.Add(ct);
                }
                catch
                { }

                if (ct.HasChildren)
                {
                    getControl(ct.Controls);
                }
            }
        }

        private void xtraTabControlMain_Click(object sender, EventArgs e)
        {

        }
    }
}