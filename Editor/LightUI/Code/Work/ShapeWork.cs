using LightUI;
using NetronLight;
using Sunny.UI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cowain
{
    public class ShapeWork
    {
        /// <summary>
        /// 从外部传进来的参数
        /// </summary>
        public List<StepRelationParam> stepRelationParams = new List<StepRelationParam>();
        private static ConcurrentQueue<string> changeStepsQueue = new ConcurrentQueue<string>();
        public ToolBlockManager toolBlockManager;
        private StepRelationParam lastStepRelationParam = null;
        public bool b_Stop = false;
        Thread thread;
        public void start()
        {
            thread = new Thread(work);
            thread.IsBackground = true;
            thread.Start();
        }
        public static void addStep(string step)
        {
            if (changeStepsQueue.Count > 500)
            {
                changeStepsQueue.Clear();
                changeStepsQueue.Enqueue(step);
            }
            else
            {
                changeStepsQueue.Enqueue(step);
            }
        }
        public bool isHead(string step)
        {
            foreach (var item in stepRelationParams)
            {
                if (item.name == step)
                {
                    return item.b_IsHead;
                }
            }
            return false;
        }
        public ToolParam getToolParam(string step)
        {
            foreach (var item in toolBlockManager.myToolBlock.myToolBlockParam.ToolParams)
            {
                if (item.ShapeBase.Text == step)
                {
                    return item;
                }
            }
            return null;
        }
        public StepRelationParam getStepRelationParam(string step)
        {
            foreach (var item in stepRelationParams)
            {
                if (item.name == step)
                {
                    return item;
                }
            }
            return null;
        }
        private void resetShapeBase(StepRelationParam stepRelationParam)
        {
            ToolParam currentToolParam = getToolParam(stepRelationParam.name);
            if (((OvalShape)currentToolParam.ShapeBase).BackGroundColor != Color.White)
            {
                ((OvalShape)currentToolParam.ShapeBase).BackGroundColor = Color.White;
            }
            foreach (var item in stepRelationParam.to)
            {
                if(item.Contains("Completed"))
                {

                }
                ToolParam toolParam = getToolParam(item);
                if (((OvalShape)toolParam.ShapeBase).BackGroundColor != Color.White)
                {
                    ((OvalShape)toolParam.ShapeBase).BackGroundColor = Color.White;
                    StepRelationParam stepRelationParam1 = getStepRelationParam(item);
                    resetShapeBase(stepRelationParam1);
                }
            }
        }
        private void work()
        {
            while (true)
            {
                Thread.Sleep(5);
                if(b_Stop)
                {
                    break;
                }
                if (changeStepsQueue.Count > 0)
                {
                    string step1 = "";
                    bool b_Result = changeStepsQueue.TryDequeue(out step1);
                    if (b_Result)
                    {
                        ToolParam toolParam = getToolParam(step1);
                        if (toolParam != null)
                        {
                            StepRelationParam stepRelationParam = getStepRelationParam(step1);
                            if (isHead(step1))
                            {
                                //如果是头，则清空所有的toList
                                resetShapeBase(stepRelationParam);
                            }
                            if (lastStepRelationParam != null)
                            {
                                if (stepRelationParam.from.Contains(lastStepRelationParam.name)&&(isHead(step1)!=true))
                                {
                                    ToolParam toolParam11 = getToolParam(lastStepRelationParam.name);
                                    ((OvalShape)toolParam11.ShapeBase).BackGroundColor = Color.LightGreen;
                                }
                            }
                            lastStepRelationParam = stepRelationParam;
                            ((OvalShape)toolParam.ShapeBase).BackGroundColor = Color.LightBlue;
                        }
                    }
                }
            }
        }
    }
}
