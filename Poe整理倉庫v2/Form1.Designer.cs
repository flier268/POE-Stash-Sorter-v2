using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Poe整理倉庫v2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.button_StartSort = new System.Windows.Forms.Button();
            this.button_ReLoadBox = new System.Windows.Forms.Button();
            this.button_Setting = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.button_CheckUpdate = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButton_langEN = new System.Windows.Forms.RadioButton();
            this.radioButton_langTW = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.label_DataGetter = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox1, resources.GetString("pictureBox1.ToolTip"));
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShowItemInfo);
            // 
            // pictureBox2
            // 
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox2, resources.GetString("pictureBox2.ToolTip"));
            this.pictureBox2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ShowItemInfo);
            // 
            // button_StartSort
            // 
            resources.ApplyResources(this.button_StartSort, "button_StartSort");
            this.button_StartSort.Name = "button_StartSort";
            this.toolTip1.SetToolTip(this.button_StartSort, resources.GetString("button_StartSort.ToolTip"));
            this.button_StartSort.UseVisualStyleBackColor = true;
            this.button_StartSort.Click += new System.EventHandler(this.button_StartSort_Click);
            // 
            // button_ReLoadBox
            // 
            resources.ApplyResources(this.button_ReLoadBox, "button_ReLoadBox");
            this.button_ReLoadBox.Name = "button_ReLoadBox";
            this.toolTip1.SetToolTip(this.button_ReLoadBox, resources.GetString("button_ReLoadBox.ToolTip"));
            this.button_ReLoadBox.UseVisualStyleBackColor = true;
            this.button_ReLoadBox.Click += new System.EventHandler(this.button_ReLoadBox_Click);
            // 
            // button_Setting
            // 
            resources.ApplyResources(this.button_Setting, "button_Setting");
            this.button_Setting.Name = "button_Setting";
            this.toolTip1.SetToolTip(this.button_Setting, resources.GetString("button_Setting.ToolTip"));
            this.button_Setting.UseVisualStyleBackColor = true;
            this.button_Setting.Click += new System.EventHandler(this.button_Setting_Click);
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Checked = true;
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton1, resources.GetString("radioButton1.ToolTip"));
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            resources.ApplyResources(this.radioButton2, "radioButton2");
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton2, resources.GetString("radioButton2.ToolTip"));
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            resources.ApplyResources(this.radioButton3, "radioButton3");
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton3, resources.GetString("radioButton3.ToolTip"));
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.radioButton5);
            this.groupBox1.Controls.Add(this.radioButton4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox1, resources.GetString("groupBox1.ToolTip"));
            // 
            // radioButton5
            // 
            resources.ApplyResources(this.radioButton5, "radioButton5");
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton5, resources.GetString("radioButton5.ToolTip"));
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            resources.ApplyResources(this.radioButton4, "radioButton4");
            this.radioButton4.Checked = true;
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton4, resources.GetString("radioButton4.ToolTip"));
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.radioButton3);
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Name = "panel1";
            this.toolTip1.SetToolTip(this.panel1, resources.GetString("panel1.ToolTip"));
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.radioButton7);
            this.groupBox2.Controls.Add(this.radioButton6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox2, resources.GetString("groupBox2.ToolTip"));
            // 
            // radioButton7
            // 
            resources.ApplyResources(this.radioButton7, "radioButton7");
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton7, resources.GetString("radioButton7.ToolTip"));
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            resources.ApplyResources(this.radioButton6, "radioButton6");
            this.radioButton6.Checked = true;
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.TabStop = true;
            this.toolTip1.SetToolTip(this.radioButton6, resources.GetString("radioButton6.ToolTip"));
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // button_CheckUpdate
            // 
            resources.ApplyResources(this.button_CheckUpdate, "button_CheckUpdate");
            this.button_CheckUpdate.Name = "button_CheckUpdate";
            this.toolTip1.SetToolTip(this.button_CheckUpdate, resources.GetString("button_CheckUpdate.ToolTip"));
            this.button_CheckUpdate.UseVisualStyleBackColor = true;
            this.button_CheckUpdate.Click += new System.EventHandler(this.button_CheckUpdate_Click);
            // 
            // linkLabel1
            // 
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.toolTip1.SetToolTip(this.linkLabel1, resources.GetString("linkLabel1.ToolTip"));
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.radioButton_langEN);
            this.groupBox3.Controls.Add(this.radioButton_langTW);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            this.toolTip1.SetToolTip(this.groupBox3, resources.GetString("groupBox3.ToolTip"));
            // 
            // radioButton_langEN
            // 
            resources.ApplyResources(this.radioButton_langEN, "radioButton_langEN");
            this.radioButton_langEN.Name = "radioButton_langEN";
            this.toolTip1.SetToolTip(this.radioButton_langEN, resources.GetString("radioButton_langEN.ToolTip"));
            this.radioButton_langEN.UseVisualStyleBackColor = true;
            this.radioButton_langEN.CheckedChanged += new System.EventHandler(this.radioButton9_CheckedChanged);
            // 
            // radioButton_langTW
            // 
            resources.ApplyResources(this.radioButton_langTW, "radioButton_langTW");
            this.radioButton_langTW.Name = "radioButton_langTW";
            this.toolTip1.SetToolTip(this.radioButton_langTW, resources.GetString("radioButton_langTW.ToolTip"));
            this.radioButton_langTW.UseVisualStyleBackColor = true;
            this.radioButton_langTW.CheckedChanged += new System.EventHandler(this.radioButton8_CheckedChanged);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.toolTip1.SetToolTip(this.button1, resources.GetString("button1.ToolTip"));
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label_DataGetter
            // 
            resources.ApplyResources(this.label_DataGetter, "label_DataGetter");
            this.label_DataGetter.ForeColor = System.Drawing.Color.Red;
            this.label_DataGetter.Name = "label_DataGetter";
            this.toolTip1.SetToolTip(this.label_DataGetter, resources.GetString("label_DataGetter.ToolTip"));
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.toolTip1.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label_DataGetter);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.button_CheckUpdate);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_Setting);
            this.Controls.Add(this.button_ReLoadBox);
            this.Controls.Add(this.button_StartSort);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.toolTip1.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_Closed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = String.Format("{0}{1}: {2}", this.Text, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            if (!File.Exists(Path.ChangeExtension(Application.ExecutablePath, ".cfg")))
            {
                MessageBox.Show(String.Format("\"{0}\" 讀取失敗，請確認後再重新啟動程式\r\nCan not load \"{0}\" ,check and restart,please.", Path.ChangeExtension(Path.GetFileName(Application.ExecutablePath), ".cfg")));
                this.Close();
                return;
            }
            using (StreamReader r = new StreamReader(Path.ChangeExtension(Application.ExecutablePath, ".cfg"), Encoding.UTF8))
            {
                Config = Setting.FromJson(r.ReadToEnd());
                if (Config == null)
                {
                    MessageBox.Show(String.Format("\"{0}\" 讀取失敗，請確認後再重新啟動程式\r\nCan not load \"{0}\" ,check and restart,please.", Path.ChangeExtension(Path.GetFileName(Application.ExecutablePath), ".cfg")));
                    this.Close();
                    return;
                }
            }
            Flier.SuperTools.Hook.KeyBoard.Global_Hook.GlobalKeyDown += new Flier.SuperTools.Hook.KeyBoard.Global_Hook.KeyEventHandlerEx(this.GlobalKeyDown);
            ItemList_Load();
#if DEBUG            
#else
            SubmitGoogleDoc();
#endif
            CheckUpdate();
            if (System.Threading.Thread.CurrentThread.CurrentUICulture.Name == "en")
                radioButton_langEN.Checked = true;
            else
                radioButton_langTW.Checked = true;
        }
        private async void SubmitGoogleDoc()
        {
            await System.Threading.Tasks.Task.Delay(0);
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                var keyval = new System.Collections.Specialized.NameValueCollection();
                keyval.Add("submit", "Submit");
                wc.Headers.Add("Origin", "https://docs.google.com");
                wc.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");

                wc.UploadValuesAsync(new Uri("https://docs.google.com/forms/d/e/1FAIpQLSfbLCkLabwKqAewyNN4swBISAHKg6HTA3CJF6a-7h-Q-rBS3Q/formResponse"), "POST", keyval, Guid.NewGuid().ToString());
            }
            catch { }
        }
        private void Form1_Closed(object sender, FormClosedEventArgs e)
        {
            
        }
        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button_StartSort;
        private System.Windows.Forms.Button button_ReLoadBox;
        private Button button_Setting;
        private RadioButton radioButton1;
        private RadioButton radioButton2;
        private RadioButton radioButton3;
        private GroupBox groupBox1;
        private RadioButton radioButton5;
        private RadioButton radioButton4;
        private Panel panel1;
        private GroupBox groupBox2;
        private RadioButton radioButton7;
        private RadioButton radioButton6;
        private Button button_CheckUpdate;
        private LinkLabel linkLabel1;
        private GroupBox groupBox3;
        private RadioButton radioButton_langEN;
        private RadioButton radioButton_langTW;
        private Button button1;
        private Label label_DataGetter;
        private Label label1;
        private ToolTip toolTip1;
    }
}