namespace Poe整理倉庫v2
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.button_TypeListDown = new System.Windows.Forms.Button();
            this.button_TypeListUp = new System.Windows.Forms.Button();
            this.listBox_TypeList = new System.Windows.Forms.ListBox();
            this.trackBar_Click = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button_Save = new System.Windows.Forms.Button();
            this.trackBar_MouseMove = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_hotkey_Start = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBox_hotkey_Stop = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.trackBar_Scan = new System.Windows.Forms.TrackBar();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.comboBox_Direction = new System.Windows.Forms.ComboBox();
            this.listBox_Priority = new System.Windows.Forms.ListBox();
            this.button_PriorityUp = new System.Windows.Forms.Button();
            this.button_PriorityDown = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Click)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_MouseMove)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Scan)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_TypeListDown
            // 
            this.button_TypeListDown.Location = new System.Drawing.Point(234, 153);
            this.button_TypeListDown.Name = "button_TypeListDown";
            this.button_TypeListDown.Size = new System.Drawing.Size(27, 23);
            this.button_TypeListDown.TabIndex = 19;
            this.button_TypeListDown.Text = "↓";
            this.button_TypeListDown.UseVisualStyleBackColor = true;
            this.button_TypeListDown.Click += new System.EventHandler(this.button_TypeListDown_Click);
            // 
            // button_TypeListUp
            // 
            this.button_TypeListUp.Location = new System.Drawing.Point(234, 123);
            this.button_TypeListUp.Name = "button_TypeListUp";
            this.button_TypeListUp.Size = new System.Drawing.Size(27, 23);
            this.button_TypeListUp.TabIndex = 18;
            this.button_TypeListUp.Text = "↑";
            this.button_TypeListUp.UseVisualStyleBackColor = true;
            this.button_TypeListUp.Click += new System.EventHandler(this.button_TypeListUp_Click);
            // 
            // listBox_TypeList
            // 
            this.listBox_TypeList.FormattingEnabled = true;
            this.listBox_TypeList.ItemHeight = 12;
            this.listBox_TypeList.Location = new System.Drawing.Point(120, 112);
            this.listBox_TypeList.Name = "listBox_TypeList";
            this.listBox_TypeList.Size = new System.Drawing.Size(108, 88);
            this.listBox_TypeList.TabIndex = 17;
            // 
            // trackBar_Click
            // 
            this.trackBar_Click.Location = new System.Drawing.Point(135, 11);
            this.trackBar_Click.Maximum = 300;
            this.trackBar_Click.Minimum = 50;
            this.trackBar_Click.Name = "trackBar_Click";
            this.trackBar_Click.Size = new System.Drawing.Size(141, 45);
            this.trackBar_Click.TabIndex = 15;
            this.trackBar_Click.Value = 100;
            this.trackBar_Click.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 117);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "2.類型順序";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 213);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 12);
            this.label2.TabIndex = 21;
            this.label2.Text = "3.快捷鍵";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 12);
            this.label4.TabIndex = 23;
            this.label4.Text = "點擊速度(拿起放下)";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(94, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(37, 22);
            this.textBox1.TabIndex = 24;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(16, 21);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(225, 16);
            this.checkBox1.TabIndex = 25;
            this.checkBox1.Text = "1.品質小於　　　　技能石放在最後面";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button_Save
            // 
            this.button_Save.Location = new System.Drawing.Point(89, 526);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(106, 42);
            this.button_Save.TabIndex = 26;
            this.button_Save.Text = "儲存";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // trackBar_MouseMove
            // 
            this.trackBar_MouseMove.Location = new System.Drawing.Point(135, 42);
            this.trackBar_MouseMove.Maximum = 300;
            this.trackBar_MouseMove.Minimum = 50;
            this.trackBar_MouseMove.Name = "trackBar_MouseMove";
            this.trackBar_MouseMove.Size = new System.Drawing.Size(141, 45);
            this.trackBar_MouseMove.TabIndex = 15;
            this.trackBar_MouseMove.Value = 50;
            this.trackBar_MouseMove.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 23;
            this.label3.Text = "滑鼠移動速度";
            // 
            // comboBox_hotkey_Start
            // 
            this.comboBox_hotkey_Start.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_hotkey_Start.FormattingEnabled = true;
            this.comboBox_hotkey_Start.Items.AddRange(new object[] {
            "None",
            "F1",
            "F2",
            "F3",
            "F4",
            "F5",
            "F6",
            "F7",
            "F8",
            "F9",
            "F11",
            "F12",
            "F13",
            "F14",
            "F15"});
            this.comboBox_hotkey_Start.Location = new System.Drawing.Point(38, 3);
            this.comboBox_hotkey_Start.Name = "comboBox_hotkey_Start";
            this.comboBox_hotkey_Start.Size = new System.Drawing.Size(58, 20);
            this.comboBox_hotkey_Start.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 7);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 28;
            this.label5.Text = "按下";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(94, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 28;
            this.label6.Text = "啟動      按下";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(225, 7);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 29;
            this.label7.Text = "停止";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.comboBox_hotkey_Stop);
            this.panel1.Controls.Add(this.comboBox_hotkey_Start);
            this.panel1.Location = new System.Drawing.Point(34, 228);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(264, 28);
            this.panel1.TabIndex = 30;
            // 
            // comboBox_hotkey_Stop
            // 
            this.comboBox_hotkey_Stop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_hotkey_Stop.FormattingEnabled = true;
            this.comboBox_hotkey_Stop.Items.AddRange(new object[] {
            "None",
            "F1",
            "F2",
            "F3",
            "F4",
            "F5",
            "F6",
            "F7",
            "F8",
            "F9",
            "F11",
            "F12",
            "F13",
            "F14",
            "F15"});
            this.comboBox_hotkey_Stop.Location = new System.Drawing.Point(165, 3);
            this.comboBox_hotkey_Stop.Name = "comboBox_hotkey_Stop";
            this.comboBox_hotkey_Stop.Size = new System.Drawing.Size(58, 20);
            this.comboBox_hotkey_Stop.TabIndex = 27;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(125, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 12);
            this.label8.TabIndex = 31;
            this.label8.Text = "快";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(125, 59);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 31;
            this.label9.Text = "快";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(270, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 32;
            this.label10.Text = "慢";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(270, 59);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(17, 12);
            this.label11.TabIndex = 32;
            this.label11.Text = "慢";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.trackBar_Scan);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.trackBar_MouseMove);
            this.groupBox1.Controls.Add(this.trackBar_Click);
            this.groupBox1.Location = new System.Drawing.Point(12, 350);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 160);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "速度";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(270, 90);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 12);
            this.label14.TabIndex = 32;
            this.label14.Text = "慢";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(125, 90);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 12);
            this.label15.TabIndex = 36;
            this.label15.Text = "快";
            // 
            // trackBar_Scan
            // 
            this.trackBar_Scan.Location = new System.Drawing.Point(135, 74);
            this.trackBar_Scan.Maximum = 300;
            this.trackBar_Scan.Minimum = 30;
            this.trackBar_Scan.Name = "trackBar_Scan";
            this.trackBar_Scan.Size = new System.Drawing.Size(141, 45);
            this.trackBar_Scan.TabIndex = 35;
            this.trackBar_Scan.Value = 50;
            this.trackBar_Scan.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(11, 91);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 34;
            this.label13.Text = "掃描速度";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.Blue;
            this.label12.Location = new System.Drawing.Point(75, 136);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(221, 12);
            this.label12.TabIndex = 33;
            this.label12.Text = "如果發現物品沒有被拿起來，請調慢一點";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.comboBox_Direction);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Location = new System.Drawing.Point(23, 267);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(335, 68);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "特殊設定";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(32, 46);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(62, 12);
            this.label17.TabIndex = 27;
            this.label17.Text = "2.排列方式";
            // 
            // comboBox_Direction
            // 
            this.comboBox_Direction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Direction.FormattingEnabled = true;
            this.comboBox_Direction.Items.AddRange(new object[] {
            "縱向排列",
            "橫向排列"});
            this.comboBox_Direction.Location = new System.Drawing.Point(94, 42);
            this.comboBox_Direction.Name = "comboBox_Direction";
            this.comboBox_Direction.Size = new System.Drawing.Size(72, 20);
            this.comboBox_Direction.TabIndex = 26;
            // 
            // listBox_Priority
            // 
            this.listBox_Priority.FormattingEnabled = true;
            this.listBox_Priority.ItemHeight = 12;
            this.listBox_Priority.Items.AddRange(new object[] {
            "地圖等級",
            "品質",
            "名稱",
            "物品等級",
            "技能寶石等級",
            "稀有度",
            "類型"});
            this.listBox_Priority.Location = new System.Drawing.Point(120, 12);
            this.listBox_Priority.Name = "listBox_Priority";
            this.listBox_Priority.Size = new System.Drawing.Size(108, 88);
            this.listBox_Priority.TabIndex = 35;
            // 
            // button_PriorityUp
            // 
            this.button_PriorityUp.Location = new System.Drawing.Point(234, 31);
            this.button_PriorityUp.Name = "button_PriorityUp";
            this.button_PriorityUp.Size = new System.Drawing.Size(27, 23);
            this.button_PriorityUp.TabIndex = 18;
            this.button_PriorityUp.Text = "↑";
            this.button_PriorityUp.UseVisualStyleBackColor = true;
            this.button_PriorityUp.Click += new System.EventHandler(this.button_PriorityUp_Click);
            // 
            // button_PriorityDown
            // 
            this.button_PriorityDown.Location = new System.Drawing.Point(234, 61);
            this.button_PriorityDown.Name = "button_PriorityDown";
            this.button_PriorityDown.Size = new System.Drawing.Size(27, 23);
            this.button_PriorityDown.TabIndex = 19;
            this.button_PriorityDown.Text = "↓";
            this.button_PriorityDown.UseVisualStyleBackColor = true;
            this.button_PriorityDown.Click += new System.EventHandler(this.button_PriorityDown_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(14, 12);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(74, 12);
            this.label16.TabIndex = 36;
            this.label16.Text = "1.排序的順序";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 580);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.listBox_Priority);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button_Save);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_PriorityDown);
            this.Controls.Add(this.button_PriorityUp);
            this.Controls.Add(this.button_TypeListDown);
            this.Controls.Add(this.button_TypeListUp);
            this.Controls.Add(this.listBox_TypeList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form3";
            this.Text = "設定";
            this.Load += new System.EventHandler(this.Form3_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Click)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_MouseMove)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_Scan)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        
        #endregion

        private System.Windows.Forms.Button button_TypeListDown;
        private System.Windows.Forms.Button button_TypeListUp;
        private System.Windows.Forms.ListBox listBox_TypeList;
        private System.Windows.Forms.TrackBar trackBar_Click;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button button_Save;
        private System.Windows.Forms.TrackBar trackBar_MouseMove;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_hotkey_Start;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBox_hotkey_Stop;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TrackBar trackBar_Scan;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ListBox listBox_Priority;
        private System.Windows.Forms.Button button_PriorityUp;
        private System.Windows.Forms.Button button_PriorityDown;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox comboBox_Direction;
    }
}