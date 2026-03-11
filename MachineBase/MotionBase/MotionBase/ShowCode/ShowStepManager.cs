using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MotionBase
{
    public enum StepType
    {
        回原,
        作业,
    }
    public class StepItem
    {
        public string instanceName;
        public StepType stepType;
        public int step;
        public StepItem(string instanceName1, StepType stepType1, int step1)
        {
            instanceName = instanceName1;
            stepType = stepType1;
            step = step1;
        }
    }
    public class ShowStepManager
    {
        private static ShowStepManager showStepManager;
        private List<ShowStep> showSteps = new List<ShowStep>();
        public ConcurrentQueue<StepItem> stepsQueue = new ConcurrentQueue<StepItem>();
        private Thread thread;
        public static object obj = new object();
        //  public Action<string, StepType, string> action;
        public static ShowStepManager instance
        {
            get
            {
                if (showStepManager == null)
                {
                    showStepManager = new ShowStepManager();
                }
                return showStepManager;
            }
        }
        public ShowStepManager()
        {
            thread = new Thread(work);
            thread.IsBackground = true;
            thread.Start();
        }
        public void addStepMSG(string instanceName1, StepType stepType1, int step)
        {
            StepItem stepItem = new StepItem(instanceName1, stepType1, step);
           // if (getShowStep(instanceName1).show)
            {
                stepsQueue.Enqueue(stepItem);
            }
        }
        public ShowStep getShowStep(string instanceName1)
        {
            lock (obj)
            {
                foreach (var item in showSteps)
                {
                    if (item.instanceName == instanceName1)
                    {
                        return item;
                    }
                }
                return null;
            }
        }
        public void addShowStep(ShowStep showStep)
        {
            lock (obj)
            {
                if (showStep.instanceName[0].ToString()=="X"|| showStep.instanceName[0].ToString()=="Y")//如果是IO，则不再添加
                {
                    return;
                }
                if(showStep.getHomeStepList().Count<=2&& showStep.getStepList().Count <= 2)//如果HomeStep和Step只有两步的，则不添加
                {
                    return;
                }
                foreach (var item in showSteps)
                {
                    if (item.instanceName == showStep.instanceName)
                    {
                        showSteps.Remove(item);
                        break;
                    }
                }
                showSteps.Add(showStep);
            }
        }
        public List<string> getAllShowStepInstanceNames()
        {
            lock (obj)
            {
                List<string> list1 = new List<string>();
                foreach (var item in showSteps)
                {
                    list1.Add(item.instanceName);
                }
                return list1;
            }
        }

        private void work()
        {
            while (true)
            {
                Thread.Sleep(5);
                if (stepsQueue.IsEmpty != true)
                {
                    StepItem stepItem;
                    bool b_Result = stepsQueue.TryDequeue(out stepItem);
                    if (b_Result)
                    {
                       try
                        {
                            ShowStep showStep = getShowStep(stepItem.instanceName);
                            string str = "";
                            if (stepItem.stepType == StepType.回原)
                            {
                                str = showStep.homeStepList[stepItem.step];
                                showStep.setcurrentHomeStep(str);
                            }
                            else
                            {
                                str = showStep.stepList[stepItem.step];
                                showStep.setcurrentStep(str);
                            }
                            //  action?.BeginInvoke(stepItem.instanceName, stepItem.stepType, str, null, null);
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }
    }
}
