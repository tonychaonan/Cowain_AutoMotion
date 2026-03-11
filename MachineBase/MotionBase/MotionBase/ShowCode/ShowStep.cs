using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionBase
{
    public class ShowStep
    {
        public bool show = false;
        public List<string> homeStepList;
        public List<string> stepList;
        string currentHomeStep = "";
        string currentStep = "";
        public string instanceName = "";
        public object obj = new object();
        public string stepEnumType = "";
        public Action<string> stepChange;
        public ShowStep(Type homeStep1, Type step1, string instanceName1)
        {
            homeStepList = Enum.GetNames(homeStep1).ToList();
            stepList = Enum.GetNames(step1).ToList();
            instanceName = instanceName1;
            stepEnumType = step1.Name.ToString();
        }
        public string getcurrentHomeStep()
        {
            lock (obj)
            {
                return currentHomeStep;
            }
        }
        public string getcurrentStep()
        {
            lock (obj)
            {
                return currentStep;
            }
        }
        public void setcurrentHomeStep(string msg)
        {
            lock (obj)
            {
                currentHomeStep = msg;
            }
        }
        public void setcurrentStep(string msg)
        {
            lock (obj)
            {
                if (currentStep != msg)
                {
                    stepChange?.BeginInvoke(msg,null,null);
                }
                currentStep = msg;
            }
        }
        public List<string> getStepList()
        {
            lock (obj)
            {
                List<string> list1 = new List<string>();
                foreach (var item in stepList)
                {
                    list1.Add(item);
                }
                return list1;
            }
        }
        public List<string> getHomeStepList()
        {
            lock (obj)
            {
                List<string> list1 = new List<string>();
                foreach (var item in homeStepList)
                {
                    list1.Add(item);
                }
                return list1;
            }
        }
    }
}
