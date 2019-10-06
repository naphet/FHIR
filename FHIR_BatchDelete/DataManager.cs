using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using FHIRDataManager.Utility;

namespace FHIRDataManager
{
    public partial class DataManager : Form
    {
        delegate void UpdateUI(String MessageInfo, int Value);

        private static String url = "";
        public DataManager()
        {
            InitializeComponent();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            btn_Delete.Enabled = false;
            txt_Url.ReadOnly = true;
            txt_Min.ReadOnly = true;
            txt_Max.ReadOnly = true;
            cbb_Type.Enabled = false;
            try
            {
                url = txt_Url.Text;
                TaskParam param = new TaskParam();
                param.Max = int.Parse(txt_Max.Text);
                param.Min = int.Parse(txt_Min.Text);

                pbProgress.Maximum = param.Max - param.Min + 1;
                pbProgress.Step = 1;
                var name = cbb_Type.Text;
                switch (name)
                {
                    case "报告":
                        param.TaskType = "DIAGNOSTICREPORT";
                        break;
                    case "病人":
                        param.TaskType = "PATIENT";
                        break;
                    case "申请":
                        param.TaskType = "SERVERREQUEST";
                        break;
                    case "影像":
                        param.TaskType = "IMAGINGSTUDY";
                        break;
                }
                var task = System.Threading.Tasks.Task.Factory.StartNew(DoDeleteTask, param)
                           .ContinueWith((t) => {
                               if (InvokeRequired)
                               {
                                   this.Invoke((Action)delegate
                                   {
                                       cbb_Type.Enabled = true;
                                       txt_Url.ReadOnly = false;
                                       txt_Min.ReadOnly = false;
                                       txt_Max.ReadOnly = false;
                                       btn_Delete.Enabled = true;
                                   });
                               }
                               else
                               {
                                   cbb_Type.Enabled = true;
                                   txt_Url.ReadOnly = false;
                                   txt_Min.ReadOnly = false;
                                   txt_Max.ReadOnly = false;
                                   btn_Delete.Enabled = true;
                               }
                           }); 
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                txt_Url.ReadOnly = false;
                txt_Min.ReadOnly = false;
                txt_Max.ReadOnly = false;
                btn_Delete.Enabled = true;
            }
        }

        void DoDeleteTask(object param)
        {
            TaskParam taskParam = param as TaskParam;
            int IdStart = taskParam.Min;
            int IdEnd = taskParam.Max;
            int i = 1;
            while (IdStart < IdEnd+1)
            {
                String postUrl =  "";
                String MessageInfo = "";
                switch(taskParam.TaskType.ToUpper().Trim())
                {  
                    case "DIAGNOSTICREPORT":
                        {
                            MessageInfo = "正在删除报告：" + IdStart.ToString();
                            postUrl = String.Format(url.TrimEnd('/') + "/DiagnosticReport/{0}?_pretty=true", IdStart.ToString());
                        }
                        break;
                    case "SERVERREQUEST":
                        {
                            MessageInfo = "正在删除申请：" + IdStart.ToString();
                            postUrl = String.Format(url.TrimEnd('/') + "/ProcedureRequest/{0}?_pretty=true", IdStart.ToString());
                        }
                        break;
                    case "IMAGINGSTUDY":
                        {
                            MessageInfo = "正在删除影像：" + IdStart.ToString();
                            postUrl = String.Format(url.TrimEnd('/') + "/ImagingStudy/{0}?_pretty=true", IdStart.ToString());
                        }
                        break;
                    case "PATIENT":
                        {
                            MessageInfo = "正在删除患者：" + IdStart.ToString();
                            postUrl = String.Format(url.TrimEnd('/') + "/Patient/{0}?_pretty=true", IdStart.ToString());
                        }
                        break;
                }
                try
                {
                    UpdateUISync(MessageInfo, i);
                    HttpHelper.HttpDelete(postUrl, "application/fhir+json");
                }
                catch(Exception ex)
                {
                     Console.WriteLine(ex.ToString());           
                }
                i++;
                IdStart++;
            }
            MessageBox.Show("完成删除");
        }

        private void DataManager_Load(object sender, EventArgs e)
        {
            cbb_Type.Items.Clear();
            cbb_Type.Items.Add("报告");
            cbb_Type.Items.Add("影像");
            cbb_Type.Items.Add("申请");
            cbb_Type.Items.Add("病人");
        }


        private void UpdateUISync(string MessageInfo, int Value)
        {
            if (InvokeRequired)
            {
                Action<string, int> updateAction = new Action<string, int>(UpdateUIProc);
                this.Invoke(new UpdateUI(UpdateUIProc), MessageInfo, Value);
            }
            else
            {
                UpdateUIProc(MessageInfo, Value);
            }
        }
        private void UpdateUIProc(string MessageInfo, int Value)
        {
            lbl_Info.Text = MessageInfo;
            pbProgress.Value = Value;
        }
    }
}
