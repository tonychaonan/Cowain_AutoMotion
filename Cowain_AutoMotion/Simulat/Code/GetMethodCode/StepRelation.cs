using Cowain_AutoMotion.FormView;
using DevExpress.XtraBars;
using DevExpress.XtraPrinting.Native.LayoutAdjustment;
using MotionBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cowain_AutoMotion
{
    public class StepRelationManager
    {
        private static StepRelationManager stepRelationManager;
        private List<StepRelation> stepRelations = new List<StepRelation>();
        public static StepRelationManager instance
        {
            get
            {
                if (stepRelationManager == null)
                {
                    stepRelationManager = new StepRelationManager();
                }
                return stepRelationManager;
            }
        }
        public void addStepRelation(string filePath1, string instanceName1)
        {
            bool b_Exist = false;
            foreach (StepRelation item in stepRelations)
            {
                if (item.instanceName == instanceName1)
                {
                    b_Exist = true;
                    break;
                }
            }
            if (b_Exist != true)
            {
                StepRelation stepRelation = new StepRelation(filePath1, instanceName1);
                stepRelations.Add(stepRelation);
            }
        }
        public StepRelation getStepRelation(string instanceName1)
        {
            foreach (StepRelation item in stepRelations)
            {
                if (item.instanceName == instanceName1)
                {
                    return item;
                }
            }
            return null;
        }
    }
    public class StepRelation
    {
        public List<StepRelationItem> stepRelationItems = new List<StepRelationItem>();
        private GetMethodCode getMethod = new GetMethodCode();
        public string instanceName = "";
        private string filePath = "";
        public StepRelation(string filePath1, string instanceName1)
        {
            filePath = filePath1;
            instanceName = instanceName1;
            creatRelation();
        }
        public void addStep(string currentStep, string nextStep)
        {
            StepRelationItem stepRelationItem1 = getStepItem(currentStep);
            if (stepRelationItem1.to.Contains(nextStep) != true)
            {
                stepRelationItem1.to.Add(nextStep);
            }

            StepRelationItem stepRelationItem2 = getStepItem(nextStep);
            if (stepRelationItem2.from.Contains(currentStep) != true)
            {
                stepRelationItem2.from.Add(currentStep);
            }
        }
        public StepRelationItem getStepItem(string name)
        {
            bool b_Exist = false;
            for (int i = 0; i < stepRelationItems.Count; i++)
            {
                if (stepRelationItems[i].name == name)
                {
                    return stepRelationItems[i];
                }
            }
            if (b_Exist != true)
            {
                StepRelationItem stepRelationItem = new StepRelationItem(name);
                stepRelationItems.Add(stepRelationItem);
                return stepRelationItem;
            }
            return null;
        }
        public void creatRelation()
        {
            ShowStep showStep = ShowStepManager.instance.getShowStep(instanceName);
            List<string> steps = showStep.getStepList();
            List<string> allCodes = getMethod.GetMethodContent(filePath, showStep.stepEnumType);
            foreach (string item in steps)
            {
                //获取代码片段，按照break划分
                List<string> codes = getCodes(allCodes, showStep.stepEnumType, item);
                List<string> nextSteps = getNextStep(codes, showStep.stepEnumType);
                if (nextSteps.Count > 0)
                {
                    foreach (string item11 in nextSteps)
                    {
                        addStep(item, item11);
                    }
                }
            }
            //给Step控件的位置赋值
            //计算有几个是没有from的
            string headStep = showStep.stepList[0];
            List<StepRelationItem> headSteps = new List<StepRelationItem>();
            foreach (var item in stepRelationItems)
            {
                if (item.from.Count == 0 || item.name == headStep)
                {
                    headSteps.Add(item);
                    item.b_IsHead = true;
                }
            }
            //开始对位置赋值
            int index = 0;
            int scaleX = 100;
            int scaleY = 100;
            foreach (var item in headSteps)
            {
                index++;
                item.col = 0;
                item.row = 0;
                setPosition(scaleX, scaleY, item, index);
                //把一个组的控件放在一个集合中
                List<StepRelationItem> groupList = new List<StepRelationItem>();
                foreach (var item11 in stepRelationItems)
                {
                    if (item11.headIndex== index)
                    {
                        item11.b_Show = false;
                        groupList.Add(item11);
                    }
                }
                for (int i = 0; i < 5; i++)
                {
                    //正向迭代
                    forwardIteration(item);
                    //反向迭代
                    foreach (var item333 in groupList)
                    {
                        item333.b_Show = false;
                    }
                    List<StepRelationItem> sortedNumbers = groupList.OrderBy(n => n.row).ToList();
                    sortedNumbers.Reverse();
                    List<StepRelationItem> lastHeads = new List<StepRelationItem>();
                    int row = sortedNumbers[0].row;
                    foreach (var item22 in sortedNumbers)
                    {
                        if (row == item22.row)
                        {
                            lastHeads.Add(item22);
                        }
                    }
                    foreach (var item33 in lastHeads)
                    {
                        reverseIteration(item33);
                    }
                }
            }
        }
        private void forwardIteration(StepRelationItem  head)
        {
            forwardIteration1(head);
        }
        private void forwardIteration1(StepRelationItem stepRelationItem)
        {
            if(stepRelationItem.name=="执行步骤2")
            {

            }
            List<int> locationXList = new List<int>();
            for (int i = 0; i < stepRelationItem.to.Count; i++)
            {
                StepRelationItem stepRelationItem1 = getStepItem(stepRelationItem.to[i]);
                locationXList.Add(stepRelationItem1.locationX);
            }
            int locationX = stepRelationItem.locationX;
            if(locationXList.Count>=2)
            {
                locationXList.Sort();
                locationX = (int)((locationXList[locationXList.Count - 1] - locationXList[0])/2.0)+ locationXList[0];
            }
            else if (locationXList.Count == 1&& stepRelationItem.col==0)
            {
                //判断谁最靠右，就用谁
                //if(locationXList[0]> locationX)
                //{
                //    locationX = locationXList[0];
                //}
            }
            stepRelationItem.locationX = locationX;
            for (int i = 0; i < stepRelationItem.to.Count; i++)
            {
                StepRelationItem stepRelationItem1 = getStepItem(stepRelationItem.to[i]);
                stepRelationItem.b_Show = true;
                if (stepRelationItem.b_Show == false || stepRelationItem1.b_Show == false)
                {
                    forwardIteration1(stepRelationItem1);
                }
            }
        }
        private void reverseIteration(StepRelationItem head)
        {
            reverseIteration1(head);
        }
        private void reverseIteration1(StepRelationItem stepRelationItem)
        {
            if (stepRelationItem.name == "执行步骤9")
            {

            }
            List<int> locationXList = new List<int>();
            for (int i = 0; i < stepRelationItem.from.Count; i++)
            {
                StepRelationItem stepRelationItem1 = getStepItem(stepRelationItem.from[i]);
                locationXList.Add(stepRelationItem1.locationX);
            }
            int locationX = stepRelationItem.locationX;
            if (locationXList.Count >= 2)
            {
                locationXList.Sort();
                locationX = (int)((locationXList[locationXList.Count - 1] - locationXList[0]) / 2.0)+ locationXList[0];
            }
            else if (locationXList.Count == 1 && stepRelationItem.col == 0)
            {
                //判断谁最靠右，就用谁
               // if (locationXList[0] > locationX)
               // {
                 //   locationX = locationXList[0];
             //   }
                // locationX = locationXList[0];
            }
            stepRelationItem.locationX = locationX;
            for (int i = 0; i < stepRelationItem.from.Count; i++)
            {
                StepRelationItem stepRelationItem1 = getStepItem(stepRelationItem.from[i]);
                stepRelationItem.b_Show = true;
                if (stepRelationItem.b_Show == false || stepRelationItem1.b_Show == false)
                {
                    reverseIteration1(stepRelationItem1);
                }
            }
        }
        //最后一个参数是为了防止死循环的
        private void setPosition(int scaleX, int scaleY, StepRelationItem stepRelationItem, int headIndex)
        {
            int row = stepRelationItem.row;
            int col = stepRelationItem.col;
            stepRelationItem.locationX = col * scaleX + headIndex * scaleX+100;
            stepRelationItem.locationY = row * scaleY+200;
            stepRelationItem.row = row;
            stepRelationItem.col = col;
            stepRelationItem.headIndex = headIndex;
            for (int i = 0; i < stepRelationItem.to.Count; i++)
            {
                StepRelationItem stepRelationItem1 = getStepItem(stepRelationItem.to[i]);
                if (stepRelationItem1.row == -1)
                {
                    stepRelationItem1.row = stepRelationItem.row + 1;
                }
                if (stepRelationItem1.col == -1)
                {
                    int maxCol = getMaxCol(stepRelationItem1.row);
                    if (maxCol >= 0 && i == 0)//说明不在同一个to集合
                    {
                        stepRelationItem1.col = maxCol + 1 + i+1;
                    }
                    else
                    {
                        stepRelationItem1.col = maxCol + i+1;
                    }
                }
                stepRelationItem.b_Show = true;
                if (stepRelationItem.b_Show == false || stepRelationItem1.b_Show == false)
                {
                    setPosition(scaleX, scaleY, stepRelationItem1, headIndex);
                }
            }
        }
        /// <summary>
        /// 在同一横排查找最大的COL
        /// </summary>
        /// <returns></returns>
        private int getMaxCol(int row)
        {
            int maxCol = -1;
            foreach (var item in stepRelationItems)
            {
                if (item.row == row)
                {
                    if (maxCol < item.col)
                    {
                        maxCol = item.col;
                    }
                }
            }
            return maxCol;
        }
        private List<string> getCodes(List<string> allCodes, string stepEnum, string step)
        {
            string currentStep = "case" + stepEnum + "." + step + ":";
            bool b_Start = false;
            List<string> codeList = new List<string>();
            foreach (string codeStr in allCodes)
            {
                //整理case
                string[] lineStrs = codeStr.Trim().Split(' ');
                string lineStrNew = "";
                for (int j = 0; j < lineStrs.Length; j++)
                {
                    if (lineStrs[j] != "")
                    {
                        lineStrNew += lineStrs[j].Trim();
                    }
                }
                if (lineStrNew == currentStep)
                {
                    b_Start = true;
                }
                if (lineStrNew.Contains("break;"))
                {
                    b_Start = false;
                }
                if (b_Start)
                {
                    codeList.Add(codeStr);
                }
                if (b_Start != true && codeList.Count > 0)
                {
                    return codeList;
                }
            }
            return new List<string>();
        }
        private List<string> getNextStep(List<string> codes, string stepEnumType)
        {
            List<string> nextSteps = new List<string>();
            for (int i = 0; i < codes.Count; i++)
            {
                string[] lineStrs = codes[i].Trim().Split(' ');
                string lineStrNew = "";
                for (int j = 0; j < lineStrs.Length; j++)
                {
                    if (lineStrs[j] != "")
                    {
                        lineStrNew += lineStrs[j].Trim();
                    }
                }
                string typeLine = "m_nStep=(int)" + stepEnumType + ".";
                if (lineStrNew.Contains(typeLine))
                {
                    string[] steps = lineStrNew.Split('.');
                    foreach (string item11 in steps)
                    {
                        if (item11.Contains(";"))
                        {
                            nextSteps.Add(item11.Split(';')[0]);
                        }
                    }
                }
            }
            return nextSteps;
        }
    }
    public class StepRelationItem
    {
        public List<string> from = new List<string>();
        public List<string> to = new List<string>();
        public int locationX = 0;
        public int locationY = 0;
        public string name = "";
        public int col = -1;
        public int row = -1;
        public int headIndex = -1;
        public bool b_Show;
        public bool b_IsHead = false;
        public StepRelationItem(string name1)
        {
            name = name1;
        }
    }
}
