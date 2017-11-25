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
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
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
            // 
            // pictureBox2
            // 
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            // 
            // button_StartSort
            // 
            resources.ApplyResources(this.button_StartSort, "button_StartSort");
            this.button_StartSort.Name = "button_StartSort";
            this.button_StartSort.UseVisualStyleBackColor = true;
            this.button_StartSort.Click += new System.EventHandler(this.button_StartSort_Click);
            // 
            // button_ReLoadBox
            // 
            resources.ApplyResources(this.button_ReLoadBox, "button_ReLoadBox");
            this.button_ReLoadBox.Name = "button_ReLoadBox";
            this.button_ReLoadBox.UseVisualStyleBackColor = true;
            this.button_ReLoadBox.Click += new System.EventHandler(this.button_ReLoadBox_Click);
            // 
            // button_Setting
            // 
            resources.ApplyResources(this.button_Setting, "button_Setting");
            this.button_Setting.Name = "button_Setting";
            this.button_Setting.UseVisualStyleBackColor = true;
            this.button_Setting.Click += new System.EventHandler(this.button_Setting_Click);
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Checked = true;
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            resources.ApplyResources(this.radioButton2, "radioButton2");
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.TabStop = true;
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            resources.ApplyResources(this.radioButton3, "radioButton3");
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.TabStop = true;
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton5);
            this.groupBox1.Controls.Add(this.radioButton4);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioButton5
            // 
            resources.ApplyResources(this.radioButton5, "radioButton5");
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.TabStop = true;
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            resources.ApplyResources(this.radioButton4, "radioButton4");
            this.radioButton4.Checked = true;
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.TabStop = true;
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton3);
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Controls.Add(this.radioButton1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButton7);
            this.groupBox2.Controls.Add(this.radioButton6);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // radioButton7
            // 
            resources.ApplyResources(this.radioButton7, "radioButton7");
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.TabStop = true;
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            resources.ApplyResources(this.radioButton6, "radioButton6");
            this.radioButton6.Checked = true;
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.TabStop = true;
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // button_CheckUpdate
            // 
            resources.ApplyResources(this.button_CheckUpdate, "button_CheckUpdate");
            this.button_CheckUpdate.Name = "button_CheckUpdate";
            this.button_CheckUpdate.UseVisualStyleBackColor = true;
            this.button_CheckUpdate.Click += new System.EventHandler(this.button_CheckUpdate_Click);
            // 
            // linkLabel1
            // 
            resources.ApplyResources(this.linkLabel1, "linkLabel1");
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.TabStop = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButton9);
            this.groupBox3.Controls.Add(this.radioButton8);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // radioButton9
            // 
            resources.ApplyResources(this.radioButton9, "radioButton9");
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.UseVisualStyleBackColor = true;
            this.radioButton9.CheckedChanged += new System.EventHandler(this.radioButton9_CheckedChanged);
            // 
            // radioButton8
            // 
            resources.ApplyResources(this.radioButton8, "radioButton8");
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.UseVisualStyleBackColor = true;
            this.radioButton8.CheckedChanged += new System.EventHandler(this.radioButton8_CheckedChanged);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            using (StreamReader r = new StreamReader(Path.ChangeExtension(Application.ExecutablePath, ".cfg"), Encoding.UTF8))
            {
                Config = Setting.FromJson(r.ReadToEnd());
            }
            Flier.SuperTools.Hook.KeyBoard.Global_Hook.GlobalKeyDown += new Flier.SuperTools.Hook.KeyBoard.Global_Hook.KeyEventHandlerEx(this.GlobalKeyDown);
            ItemList_Load();
#if DEBUG            
#else
            SubmitGoogleDoc();
#endif
            CheckUpdate();
            if (System.Threading.Thread.CurrentThread.CurrentUICulture == new CultureInfo("en"))
                radioButton9.Checked = true;
            else
                radioButton8.Checked = true;
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
        private RadioButton radioButton9;
        private RadioButton radioButton8;
        private Button button1;
    }
}