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
using MotionBase;
using Cowain_AutoMotion.Flow;
using System.Threading;
using Cowain_AutoMotion;
using Cowain_Machine;

namespace Cowain_Form.FormView
{
    public partial class ControlForm : DevExpress.XtraEditors.XtraForm
    {
        public ControlForm(ref clsMachine pM)
        {
            InitializeComponent();

            pMachine = pM;

        }

        #region 自定义参数
        public clsMachine pMachine;
        public enum enformList
        {
            enHomeForm = 0,
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

        DataTable m_ParamTable = new DataTable();
        DataTable m_ViewTable = new DataTable();

        #endregion

        #region 自定义方法

        #endregion

        private void ControlForm_Load(object sender, EventArgs e)
        {
            //LoadForm();
            InitailDataTable();
            this.gridView.OptionsBehavior.Editable = false;
            
        }

        #region  dockPanel中代码

        private void  InitailDataTable()
        {
            //#region 数据编辑m_ParamTable
            m_ParamTable.Columns.Add("Enable", Type.GetType("System.Boolean"));
            m_ParamTable.Columns.Add("Key Name", Type.GetType("System.String"));
            m_ParamTable.Columns.Add("LSL", Type.GetType("System.Decimal"));
            m_ParamTable.Columns.Add("USL", Type.GetType("System.Decimal"));
            m_ParamTable.Columns.Add("Standard", Type.GetType("System.Decimal"));

            //m_ParamTable.Rows.Clear();

            //m_ParamTable.Rows.Add(MESDataDefine.MESParamDatas.strABRateEnable[0] == "1" ? true : false, "F_ABRate", ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrABRateLimitS[0][0]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrABRateLimitS[0][1]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrABRateS[0]));
            //m_ParamTable.Rows.Add(MESDataDefine.MESParamDatas.strAPressureEnable[0] == "1" ? true : false, "F_APressure", ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrAPressureLimitS[0][0]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrAPressureLimitS[0][1]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrAPressureS[0]));
            //m_ParamTable.Rows.Add(MESDataDefine.MESParamDatas.strBPressureEnable[0] == "1" ? true : false, "F_BPressure", ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrBPressureLimitS[0][0]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrBPressureLimitS[0][1]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrBPressureS[0]));
            //m_ParamTable.Rows.Add(MESDataDefine.MESParamDatas.strNordPressureEnable[0] == "1" ? true : false, "F_Pressure", ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrNordPressureLimitS[0][0]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrNordPressureLimitS[0][1]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrNordPressureS[0]));
            //m_ParamTable.Rows.Add(MESDataDefine.MESParamDatas.strNordsonTempEnable[0] == "1" ? true : false, "F_Nozzle", ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrNordsonTempLimitS[0][0]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrNordsonTempLimitS[0][1]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrNordsonTempS[0]));
            //m_ParamTable.Rows.Add(MESDataDefine.MESParamDatas.strTubeNordsonTempEnable[0] == "1" ? true : false, "F_Tube", ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrTubeNordsonTempLimitS[0][0]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrTubeNordsonTempLimitS[0][1]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrTubeNordsonTempS[0]));
            //m_ParamTable.Rows.Add(MESDataDefine.MESParamDatas.strTubeNordsonTempEnable[0] == "1" ? true : false, "F_CartridgePressure", ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrCartridgeLimitS[0][0]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrCartridgeLimitS[0][1]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrCartridgeS[0]));

            //m_ParamTable.Rows.Add(MESDataDefine.MESParamDatas.strABRateEnable[1] == "1" ? true : false, "B_ABRate", ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrABRateLimitS[1][0]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrABRateLimitS[1][1]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrABRateS[1]));
            //m_ParamTable.Rows.Add(MESDataDefine.MESParamDatas.strAPressureEnable[1] == "1" ? true : false, "B_APressure", ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrAPressureLimitS[1][0]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrAPressureLimitS[1][1]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrAPressureS[1]));
            //m_ParamTable.Rows.Add(MESDataDefine.MESParamDatas.strBPressureEnable[1] == "1" ? true : false, "B_BPressure", ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrBPressureLimitS[1][0]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrBPressureLimitS[1][1]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrBPressureS[1]));
            //m_ParamTable.Rows.Add(MESDataDefine.MESParamDatas.strNordPressureEnable[1] == "1" ? true : false, "B_Pressure", ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrNordPressureLimitS[1][0]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrNordPressureLimitS[1][1]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrNordPressureS[1]));
            //m_ParamTable.Rows.Add(MESDataDefine.MESParamDatas.strNordsonTempEnable[1] == "1" ? true : false, "B_Nozzle", ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrNordsonTempLimitS[1][0]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrNordsonTempLimitS[1][1]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrNordsonTempS[1]));
            //m_ParamTable.Rows.Add(MESDataDefine.MESParamDatas.strTubeNordsonTempEnable[1] == "1" ? true : false, "B_Tube", ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrTubeNordsonTempLimitS[1][0]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrTubeNordsonTempLimitS[1][1]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrTubeNordsonTempS[1]));
            //m_ParamTable.Rows.Add(MESDataDefine.MESParamDatas.strTubeNordsonTempEnable[0] == "1" ? true : false, "B_CartridgePressure", ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrCartridgeLimitS[1][0]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrCartridgeLimitS[1][1]), ConvertHelper.GetDef_Dec(MESDataDefine.MESParamDatas.StrCartridgeS[1]));

            //gridControl.DataSource = m_ParamTable;
            //#endregion

            //#region 数据查看m_ViewTable
            //m_ViewTable.Columns.Add("Key Name", Type.GetType("System.String"));
            //m_ViewTable.Columns.Add("LSL", Type.GetType("System.Decimal"));
            //m_ViewTable.Columns.Add("USL", Type.GetType("System.Decimal"));
            //m_ViewTable.Rows.Clear();
            //if (MESDataDefine.MESParamDatas.strABRateEnable[0] == "1")
            //{
            //    m_ViewTable.Rows.Add("F_ABRate", MESDataDefine.MESParamDatas.StrABRateLimitS[0][0], MESDataDefine.MESParamDatas.StrABRateLimitS[0][1]);
            //}
            //if (MESDataDefine.MESParamDatas.strAPressureEnable[0] == "1")
            //{
            //    m_ViewTable.Rows.Add("F_APressure", MESDataDefine.MESParamDatas.StrAPressureLimitS[0][0], MESDataDefine.MESParamDatas.StrAPressureLimitS[0][1]);
            //}
            //if (MESDataDefine.MESParamDatas.strBPressureEnable[0] == "1")
            //{
            //    m_ViewTable.Rows.Add("F_BPressure", MESDataDefine.MESParamDatas.StrBPressureLimitS[0][0], MESDataDefine.MESParamDatas.StrBPressureLimitS[0][1]);
            //}
            //if (MESDataDefine.MESParamDatas.strNordPressureEnable[0] == "1")
            //{
            //    m_ViewTable.Rows.Add("F_Pressure", MESDataDefine.MESParamDatas.StrNordPressureLimitS[0][0], MESDataDefine.MESParamDatas.StrNordPressureLimitS[0][1]);
            //}
            //if (MESDataDefine.MESParamDatas.strNordsonTempEnable[0] == "1")
            //{
            //    m_ViewTable.Rows.Add("F_Nozzle", MESDataDefine.MESParamDatas.StrNordsonTempLimitS[0][0], MESDataDefine.MESParamDatas.StrNordsonTempLimitS[0][1]);
            //}
            //if (MESDataDefine.MESParamDatas.strTubeNordsonTempEnable[0] == "1")
            //{
            //    m_ViewTable.Rows.Add("F_Tube", MESDataDefine.MESParamDatas.StrTubeNordsonTempLimitS[0][0], MESDataDefine.MESParamDatas.StrTubeNordsonTempLimitS[0][1]);
            //}
            //if (MESDataDefine.MESParamDatas.strCartridgeEnable[0] == "1")
            //{
            //    m_ViewTable.Rows.Add("F_CartridgePressure", MESDataDefine.MESParamDatas.StrCartridgeLimitS[0][0], MESDataDefine.MESParamDatas.StrCartridgeLimitS[0][1]);
            //}

            //if (MESDataDefine.MESParamDatas.strABRateEnable[1] == "1")
            //{
            //    m_ViewTable.Rows.Add("B_ABRate", MESDataDefine.MESParamDatas.StrABRateLimitS[1][0], MESDataDefine.MESParamDatas.StrABRateLimitS[1][1]);
            //}
            //if (MESDataDefine.MESParamDatas.strAPressureEnable[1] == "1")
            //{
            //    m_ViewTable.Rows.Add("B_APressure", MESDataDefine.MESParamDatas.StrAPressureLimitS[1][0], MESDataDefine.MESParamDatas.StrAPressureLimitS[1][1]);
            //}
            //if (MESDataDefine.MESParamDatas.strBPressureEnable[1] == "1")
            //{
            //    m_ViewTable.Rows.Add("B_BPressure", MESDataDefine.MESParamDatas.StrBPressureLimitS[1][0], MESDataDefine.MESParamDatas.StrBPressureLimitS[1][1]);
            //}
            //if (MESDataDefine.MESParamDatas.strNordPressureEnable[1] == "1")
            //{
            //    m_ViewTable.Rows.Add("B_Pressure", MESDataDefine.MESParamDatas.StrNordPressureLimitS[1][0], MESDataDefine.MESParamDatas.StrNordPressureLimitS[1][1]);
            //}
            //if (MESDataDefine.MESParamDatas.strNordsonTempEnable[1] == "1")
            //{
            //    m_ViewTable.Rows.Add("B_Nozzle", MESDataDefine.MESParamDatas.StrNordsonTempLimitS[1][0], MESDataDefine.MESParamDatas.StrNordsonTempLimitS[1][1]);
            //}
            //if (MESDataDefine.MESParamDatas.strTubeNordsonTempEnable[1] == "1")
            //{
            //    m_ViewTable.Rows.Add("B_Tube", MESDataDefine.MESParamDatas.StrTubeNordsonTempLimitS[1][0], MESDataDefine.MESParamDatas.StrTubeNordsonTempLimitS[1][1]);
            //}
            //if (MESDataDefine.MESParamDatas.strCartridgeEnable[1] == "1")
            //{
            //    m_ViewTable.Rows.Add("B_CartridgePressure", MESDataDefine.MESParamDatas.StrCartridgeLimitS[1][0], MESDataDefine.MESParamDatas.StrCartridgeLimitS[1][1]);
            //}

            foreach (var item in MachineDataDefine.MachineCfgS.SpecClasss)
            {
                m_ParamTable.Rows.Add(item.b_Use, item.name, item.LSpec, item.USpec, item.SValue);
            }
            gridControl.DataSource = m_ParamTable;


            m_ViewTable.Columns.Add("Key Name", Type.GetType("System.String"));
            m_ViewTable.Columns.Add("LSL", Type.GetType("System.Decimal"));
            m_ViewTable.Columns.Add("USL", Type.GetType("System.Decimal"));
            m_ViewTable.Rows.Clear();
            foreach (var item in MachineDataDefine.MachineCfgS.SpecClasss)
            {
                if(item.b_Use)
                {
                    m_ViewTable.Rows.Add(item.name, item.LSpec, item.USpec);
                }
            }
            gridControl_View.DataSource = m_ViewTable;

            #endregion
        }

        private void RefreshData()
        {
            m_ViewTable.Rows.Clear();
            string name = string.Empty;
            decimal lsl = 0;
            decimal usl = 0;
            decimal standard = 0;
            bool enable = false;

            for (int i = 0; i < m_ParamTable.Rows.Count; i++)
            {
                enable = ConvertHelper.GetDef_Bool(m_ParamTable.Rows[i]["Enable"]);
                if (enable)
                {
                    name = ConvertHelper.GetDef_Str(m_ParamTable.Rows[i]["Key Name"]);
                    lsl = ConvertHelper.GetDef_Dec(m_ParamTable.Rows[i]["LSL"]);
                    usl = ConvertHelper.GetDef_Dec(m_ParamTable.Rows[i]["USL"]);

                    m_ViewTable.Rows.Add(name, lsl, usl);
                }
            }
        }


        /// <summary>
        /// 编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (pMachine.m_LoginUser != Sys_Define.enPasswordType.Maker)
                return;
            if (this.gridView.OptionsBehavior.Editable == false)
            {
                this.gridView.OptionsBehavior.Editable = true;

                //设置某列不可以编辑
                int index = 0;
                foreach (var item in gridView.Columns)
                {
                    if (gridView.Columns[index].FieldName == "Key Name")
                    {
                        gridView.Columns[index].OptionsColumn.AllowEdit = false;
                    }
                    index++;
                }
            }
            //else
            //{
            //    this.gridView.OptionsBehavior.Editable = false;
            //}
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pMachine.m_LoginUser != Sys_Define.enPasswordType.Maker)
                return;

            //if (this.gridView.OptionsBehavior.Editable == true)
            //{
            //    int rowCount = this.gridView.RowCount;
            //    DataRow dataRow;
            //    string firstCellName = "";
            //    string lslValue = "";
            //    string uslValue = "";
            //    string standard = "";
            //    bool use = false;
            //    //判断keyName是否存在以及
            //    for (int i = 0; i < rowCount; i++)
            //    {
            //        dataRow = gridView.GetDataRow(i);
            //        firstCellName = dataRow["Key Name"].ToString();
            //        if (firstCellName != null && firstCellName != string.Empty)
            //        {
            //            lslValue = dataRow["LSL"].ToString();
            //            uslValue = dataRow["USL"].ToString();
            //            use = ConvertHelper.GetDef_Bool(dataRow["Enable"]);
            //            standard = dataRow["Standard"].ToString();

            //            if (string.IsNullOrWhiteSpace(lslValue) || string.IsNullOrWhiteSpace(uslValue) || string.IsNullOrWhiteSpace(standard))
            //            {
            //                MsgBoxHelper.DxMsgShowErr("第" + (i + 1).ToString() + "行有值为空，保存失败！");
            //                return;
            //            }
            //            switch (firstCellName)
            //            {
            //                case "F_ABRate":
            //                    MESDataDefine.MESParamDatas.StrABRateLimitS[0][0] = lslValue;
            //                    MESDataDefine.MESParamDatas.StrABRateLimitS[0][1] = uslValue;
            //                    MESDataDefine.MESParamDatas.strABRateEnable[0] = use ? "1" : "0";
            //                    MESDataDefine.MESParamDatas.StrABRateS[0] = standard;
            //                    break;
            //                case "F_APressure":
            //                    MESDataDefine.MESParamDatas.StrAPressureLimitS[0][0] = lslValue;
            //                    MESDataDefine.MESParamDatas.StrAPressureLimitS[0][1] = uslValue;
            //                    MESDataDefine.MESParamDatas.strAPressureEnable[0] = use ? "1" : "0";
            //                    MESDataDefine.MESParamDatas.StrAPressureS[0] = standard;
            //                    break;
            //                case "F_BPressure":
            //                    MESDataDefine.MESParamDatas.StrBPressureLimitS[0][0] = lslValue;
            //                    MESDataDefine.MESParamDatas.StrBPressureLimitS[0][1] = uslValue;
            //                    MESDataDefine.MESParamDatas.strBPressureEnable[0] = use ? "1" : "0";
            //                    MESDataDefine.MESParamDatas.StrBPressureS[0] = standard;
            //                    break;
            //                case "F_Pressure":
            //                    MESDataDefine.MESParamDatas.StrNordPressureLimitS[0][0] = lslValue;
            //                    MESDataDefine.MESParamDatas.StrNordPressureLimitS[0][1] = uslValue;
            //                    MESDataDefine.MESParamDatas.strNordPressureEnable[0] = use ? "1" : "0";
            //                    MESDataDefine.MESParamDatas.StrNordPressureS[0] = standard;
            //                    break;
            //                case "F_Nozzle":
            //                    MESDataDefine.MESParamDatas.StrNordsonTempLimitS[0][0] = lslValue;
            //                    MESDataDefine.MESParamDatas.StrNordsonTempLimitS[0][1] = uslValue;
            //                    MESDataDefine.MESParamDatas.strNordsonTempEnable[0] = use ? "1" : "0";
            //                    MESDataDefine.MESParamDatas.StrNordsonTempS[0] = standard;
            //                    break;
            //                case "F_Tube":
            //                    MESDataDefine.MESParamDatas.StrTubeNordsonTempLimitS[0][0] = lslValue;
            //                    MESDataDefine.MESParamDatas.StrTubeNordsonTempLimitS[0][1] = uslValue;
            //                    MESDataDefine.MESParamDatas.strTubeNordsonTempEnable[0] = use ? "1" : "0";
            //                    MESDataDefine.MESParamDatas.StrTubeNordsonTempS[0] = standard;
            //                    break;
            //                case "F_CartridgePressure":
            //                    MESDataDefine.MESParamDatas.StrCartridgeLimitS[0][0] = lslValue;
            //                    MESDataDefine.MESParamDatas.StrCartridgeLimitS[0][1] = uslValue;
            //                    MESDataDefine.MESParamDatas.strCartridgeEnable[0] = use ? "1" : "0";
            //                    MESDataDefine.MESParamDatas.StrCartridgeS[0] = standard;
            //                    break;

            //                case "B_ABRate":
            //                    MESDataDefine.MESParamDatas.StrABRateLimitS[1][0] = lslValue;
            //                    MESDataDefine.MESParamDatas.StrABRateLimitS[1][1] = uslValue;
            //                    MESDataDefine.MESParamDatas.strABRateEnable[1] = use ? "1" : "0";
            //                    MESDataDefine.MESParamDatas.StrABRateS[1] = standard;
            //                    break;
            //                case "B_APressure":
            //                    MESDataDefine.MESParamDatas.StrAPressureLimitS[1][0] = lslValue;
            //                    MESDataDefine.MESParamDatas.StrAPressureLimitS[1][1] = uslValue;
            //                    MESDataDefine.MESParamDatas.strAPressureEnable[1] = use ? "1" : "0";
            //                    MESDataDefine.MESParamDatas.StrAPressureS[1] = standard;
            //                    break;
            //                case "B_BPressure":
            //                    MESDataDefine.MESParamDatas.StrBPressureLimitS[1][0] = lslValue;
            //                    MESDataDefine.MESParamDatas.StrBPressureLimitS[1][1] = uslValue;
            //                    MESDataDefine.MESParamDatas.strBPressureEnable[1] = use ? "1" : "0";
            //                    MESDataDefine.MESParamDatas.StrBPressureS[1] = standard;
            //                    break;
            //                case "B_Pressure":
            //                    MESDataDefine.MESParamDatas.StrNordPressureLimitS[1][0] = lslValue;
            //                    MESDataDefine.MESParamDatas.StrNordPressureLimitS[1][1] = uslValue;
            //                    MESDataDefine.MESParamDatas.strNordPressureEnable[1] = use ? "1" : "0";
            //                    MESDataDefine.MESParamDatas.StrNordPressureS[1] = standard;
            //                    break;
            //                case "B_Nozzle":
            //                    MESDataDefine.MESParamDatas.StrNordsonTempLimitS[1][0] = lslValue;
            //                    MESDataDefine.MESParamDatas.StrNordsonTempLimitS[1][1] = uslValue;
            //                    MESDataDefine.MESParamDatas.strNordsonTempEnable[1] = use ? "1" : "0";
            //                    MESDataDefine.MESParamDatas.StrNordsonTempS[1] = standard;
            //                    break;
            //                case "B_Tube":
            //                    MESDataDefine.MESParamDatas.StrTubeNordsonTempLimitS[1][0] = lslValue;
            //                    MESDataDefine.MESParamDatas.StrTubeNordsonTempLimitS[1][1] = uslValue;
            //                    MESDataDefine.MESParamDatas.strTubeNordsonTempEnable[1] = use ? "1" : "0";
            //                    MESDataDefine.MESParamDatas.StrTubeNordsonTempS[1] = standard;
            //                    break;
            //                case "B_CartridgePressure":
            //                    MESDataDefine.MESParamDatas.StrCartridgeLimitS[1][0] = lslValue;
            //                    MESDataDefine.MESParamDatas.StrCartridgeLimitS[1][1] = uslValue;
            //                    MESDataDefine.MESParamDatas.strCartridgeEnable[1] = use ? "1" : "0";
            //                    MESDataDefine.MESParamDatas.StrCartridgeS[1] = standard;
            //                    break;
            //                default:

            //                    MsgBoxHelper.DxMsgShowErr("第" + (i + 1).ToString() + "行的数据保存地址不存在，请在程序中添加！");
            //                    break;
            //            }
            //        }
            //        else
            //        {
            //            MsgBoxHelper.DxMsgShowErr("第" + (i + 1).ToString() + "行的Key Name为空，保存失败！");
            //            return;
            //        }
             //   }

                //保存数据到json文件中
              //  MESDataDefine.MESParamDatas.WriteParams(MESDataDefine.MESParamDatas);

                //将gridView编辑属性置为false
                this.gridView.OptionsBehavior.Editable = false;
         //   }
            RefreshData();
        }

        /// <summary>
        /// 双击gridView单元格事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            if (this.gridView.OptionsBehavior.Editable == false)
            {
                MsgBoxHelper.DxMsgShowErr("如需编辑，请点击一下 Edit 按钮！");
            }
        }

        private void ControlForm_VisibleChanged(object sender, EventArgs e)
        {
            if (pMachine.GetisAutoing())
            {
                dockPanel1.Enabled = false;
            }
            else
            {
                dockPanel1.Enabled = true;
            }
        }
    }
}