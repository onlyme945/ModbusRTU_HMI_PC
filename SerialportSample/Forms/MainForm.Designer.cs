namespace SerialportSample
{
    partial class SerialportSampleForm
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
            this.MainFormStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.MainFormMenuStrip = new System.Windows.Forms.MenuStrip();
            this.通讯设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonModbus1 = new SerialportSample.ButtonModbus();
            this.bitStatus2 = new SerialportSample.Custom_Controls.BitStatus();
            this.bitStatus1 = new SerialportSample.Custom_Controls.BitStatus();
            this.modbusView5 = new SerialportSample.TextBoxModbus();
            this.modbusView4 = new SerialportSample.TextBoxModbus();
            this.modbusView3 = new SerialportSample.TextBoxModbus();
            this.modbusView2 = new SerialportSample.TextBoxModbus();
            this.modbusView1 = new SerialportSample.TextBoxModbus();
            this.MainFormStatusStrip.SuspendLayout();
            this.MainFormMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainFormStatusStrip
            // 
            this.MainFormStatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainFormStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.MainFormStatusStrip.Location = new System.Drawing.Point(0, 446);
            this.MainFormStatusStrip.Name = "MainFormStatusStrip";
            this.MainFormStatusStrip.Size = new System.Drawing.Size(573, 22);
            this.MainFormStatusStrip.TabIndex = 17;
            this.MainFormStatusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel1.Text = "  ";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(436, 334);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 25;
            this.button2.Text = "刷新标志位";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(16, 280);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(348, 122);
            this.richTextBox1.TabIndex = 44;
            this.richTextBox1.Text = "";
            // 
            // MainFormMenuStrip
            // 
            this.MainFormMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.通讯设置ToolStripMenuItem});
            this.MainFormMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainFormMenuStrip.Name = "MainFormMenuStrip";
            this.MainFormMenuStrip.Size = new System.Drawing.Size(573, 25);
            this.MainFormMenuStrip.TabIndex = 55;
            this.MainFormMenuStrip.Text = "menuStrip1";
            // 
            // 通讯设置ToolStripMenuItem
            // 
            this.通讯设置ToolStripMenuItem.Name = "通讯设置ToolStripMenuItem";
            this.通讯设置ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.通讯设置ToolStripMenuItem.Text = "通讯设置";
            this.通讯设置ToolStripMenuItem.Click += new System.EventHandler(this.通讯设置ToolStripMenuItem_Click);
            // 
            // buttonModbus1
            // 
            this.buttonModbus1.ButtonClickAction = SerialportSample.ButtonModbus.ModbusButtonClickActionEnum.Toggle;
            this.buttonModbus1.EnablePeriodRefresh = true;
            this.buttonModbus1.Location = new System.Drawing.Point(207, 151);
            this.buttonModbus1.Name = "buttonModbus1";
            this.buttonModbus1.ReadAddress = ((ushort)(0));
            this.buttonModbus1.RefreshPeriod = 100D;
            this.buttonModbus1.Size = new System.Drawing.Size(75, 23);
            this.buttonModbus1.StationID = ((byte)(1));
            this.buttonModbus1.TabIndex = 54;
            this.buttonModbus1.WriteAddress = ((ushort)(0));
            // 
            // bitStatus2
            // 
            this.bitStatus2.BackColor = System.Drawing.Color.Gray;
            this.bitStatus2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bitStatus2.EnablePeriodRequest = true;
            this.bitStatus2.Location = new System.Drawing.Point(436, 195);
            this.bitStatus2.Name = "bitStatus2";
            this.bitStatus2.ReadAddress = ((ushort)(1));
            this.bitStatus2.ReadDataLengthInBit = ((byte)(1));
            this.bitStatus2.ReadFuncCode = SerialportSample.Custom_Controls.BitStatus.ReadFunctionCodeEnum.ReadCoils;
            this.bitStatus2.RefreshPeriod = 100D;
            this.bitStatus2.Size = new System.Drawing.Size(26, 25);
            this.bitStatus2.StationID = ((byte)(1));
            this.bitStatus2.TabIndex = 53;
            // 
            // bitStatus1
            // 
            this.bitStatus1.BackColor = System.Drawing.Color.Gray;
            this.bitStatus1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bitStatus1.EnablePeriodRequest = true;
            this.bitStatus1.Location = new System.Drawing.Point(436, 151);
            this.bitStatus1.Name = "bitStatus1";
            this.bitStatus1.ReadAddress = ((ushort)(0));
            this.bitStatus1.ReadDataLengthInBit = ((byte)(1));
            this.bitStatus1.ReadFuncCode = SerialportSample.Custom_Controls.BitStatus.ReadFunctionCodeEnum.ReadCoils;
            this.bitStatus1.RefreshPeriod = 100D;
            this.bitStatus1.Size = new System.Drawing.Size(26, 25);
            this.bitStatus1.StationID = ((byte)(1));
            this.bitStatus1.TabIndex = 52;
            // 
            // modbusView5
            // 
            this.modbusView5.EnablePeriodRequest = true;
            this.modbusView5.Location = new System.Drawing.Point(27, 209);
            this.modbusView5.MaxValue = 100D;
            this.modbusView5.MinValue = 1D;
            this.modbusView5.Name = "modbusView5";
            this.modbusView5.ReadAddress = ((ushort)(4));
            this.modbusView5.ReadDataLengthInWord = ((byte)(1));
            this.modbusView5.ReadDataType = SerialportSample.TextBoxModbus.ReadDataTypeEnum.UINT16;
            this.modbusView5.ReadFuncCode = SerialportSample.TextBoxModbus.ReadFunctionCodeEnum.ReadStorageRegs;
            this.modbusView5.RequestPeriod = 100D;
            this.modbusView5.Size = new System.Drawing.Size(100, 21);
            this.modbusView5.StationID = ((byte)(1));
            this.modbusView5.TabIndex = 51;
            this.modbusView5.Text = "0";
            this.modbusView5.WriteAddress = ((ushort)(4));
            this.modbusView5.WriteDataLengthInWord = ((byte)(1));
            this.modbusView5.WriteDataType = SerialportSample.TextBoxModbus.WriteDataTypeEnum.UINT16;
            this.modbusView5.WriteFuncCode = SerialportSample.TextBoxModbus.WriteFunctionCodeEnum.WriteRegs;
            // 
            // modbusView4
            // 
            this.modbusView4.EnablePeriodRequest = true;
            this.modbusView4.Location = new System.Drawing.Point(27, 182);
            this.modbusView4.MaxValue = 100D;
            this.modbusView4.MinValue = 1D;
            this.modbusView4.Name = "modbusView4";
            this.modbusView4.ReadAddress = ((ushort)(3));
            this.modbusView4.ReadDataLengthInWord = ((byte)(1));
            this.modbusView4.ReadDataType = SerialportSample.TextBoxModbus.ReadDataTypeEnum.UINT16;
            this.modbusView4.ReadFuncCode = SerialportSample.TextBoxModbus.ReadFunctionCodeEnum.ReadStorageRegs;
            this.modbusView4.RequestPeriod = 100D;
            this.modbusView4.Size = new System.Drawing.Size(100, 21);
            this.modbusView4.StationID = ((byte)(1));
            this.modbusView4.TabIndex = 50;
            this.modbusView4.Text = "0";
            this.modbusView4.WriteAddress = ((ushort)(3));
            this.modbusView4.WriteDataLengthInWord = ((byte)(1));
            this.modbusView4.WriteDataType = SerialportSample.TextBoxModbus.WriteDataTypeEnum.UINT16;
            this.modbusView4.WriteFuncCode = SerialportSample.TextBoxModbus.WriteFunctionCodeEnum.WriteRegs;
            // 
            // modbusView3
            // 
            this.modbusView3.EnablePeriodRequest = true;
            this.modbusView3.Location = new System.Drawing.Point(27, 155);
            this.modbusView3.MaxValue = 100D;
            this.modbusView3.MinValue = 1D;
            this.modbusView3.Name = "modbusView3";
            this.modbusView3.ReadAddress = ((ushort)(2));
            this.modbusView3.ReadDataLengthInWord = ((byte)(1));
            this.modbusView3.ReadDataType = SerialportSample.TextBoxModbus.ReadDataTypeEnum.UINT16;
            this.modbusView3.ReadFuncCode = SerialportSample.TextBoxModbus.ReadFunctionCodeEnum.ReadStorageRegs;
            this.modbusView3.RequestPeriod = 100D;
            this.modbusView3.Size = new System.Drawing.Size(100, 21);
            this.modbusView3.StationID = ((byte)(1));
            this.modbusView3.TabIndex = 49;
            this.modbusView3.Text = "0";
            this.modbusView3.WriteAddress = ((ushort)(2));
            this.modbusView3.WriteDataLengthInWord = ((byte)(1));
            this.modbusView3.WriteDataType = SerialportSample.TextBoxModbus.WriteDataTypeEnum.UINT16;
            this.modbusView3.WriteFuncCode = SerialportSample.TextBoxModbus.WriteFunctionCodeEnum.WriteRegs;
            // 
            // modbusView2
            // 
            this.modbusView2.EnablePeriodRequest = true;
            this.modbusView2.Location = new System.Drawing.Point(27, 128);
            this.modbusView2.MaxValue = 100D;
            this.modbusView2.MinValue = 1D;
            this.modbusView2.Name = "modbusView2";
            this.modbusView2.ReadAddress = ((ushort)(1));
            this.modbusView2.ReadDataLengthInWord = ((byte)(1));
            this.modbusView2.ReadDataType = SerialportSample.TextBoxModbus.ReadDataTypeEnum.UINT16;
            this.modbusView2.ReadFuncCode = SerialportSample.TextBoxModbus.ReadFunctionCodeEnum.ReadStorageRegs;
            this.modbusView2.RequestPeriod = 100D;
            this.modbusView2.Size = new System.Drawing.Size(100, 21);
            this.modbusView2.StationID = ((byte)(1));
            this.modbusView2.TabIndex = 48;
            this.modbusView2.Text = "0";
            this.modbusView2.WriteAddress = ((ushort)(1));
            this.modbusView2.WriteDataLengthInWord = ((byte)(1));
            this.modbusView2.WriteDataType = SerialportSample.TextBoxModbus.WriteDataTypeEnum.UINT16;
            this.modbusView2.WriteFuncCode = SerialportSample.TextBoxModbus.WriteFunctionCodeEnum.WriteRegs;
            // 
            // modbusView1
            // 
            this.modbusView1.EnablePeriodRequest = true;
            this.modbusView1.Location = new System.Drawing.Point(27, 101);
            this.modbusView1.MaxValue = 100D;
            this.modbusView1.MinValue = 1D;
            this.modbusView1.Name = "modbusView1";
            this.modbusView1.ReadAddress = ((ushort)(0));
            this.modbusView1.ReadDataLengthInWord = ((byte)(1));
            this.modbusView1.ReadDataType = SerialportSample.TextBoxModbus.ReadDataTypeEnum.UINT16;
            this.modbusView1.ReadFuncCode = SerialportSample.TextBoxModbus.ReadFunctionCodeEnum.ReadStorageRegs;
            this.modbusView1.RequestPeriod = 100D;
            this.modbusView1.Size = new System.Drawing.Size(100, 21);
            this.modbusView1.StationID = ((byte)(1));
            this.modbusView1.TabIndex = 47;
            this.modbusView1.Text = "0";
            this.modbusView1.WriteAddress = ((ushort)(0));
            this.modbusView1.WriteDataLengthInWord = ((byte)(1));
            this.modbusView1.WriteDataType = SerialportSample.TextBoxModbus.WriteDataTypeEnum.UINT16;
            this.modbusView1.WriteFuncCode = SerialportSample.TextBoxModbus.WriteFunctionCodeEnum.WriteRegs;
            // 
            // SerialportSampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 468);
            this.Controls.Add(this.buttonModbus1);
            this.Controls.Add(this.bitStatus2);
            this.Controls.Add(this.bitStatus1);
            this.Controls.Add(this.modbusView5);
            this.Controls.Add(this.modbusView4);
            this.Controls.Add(this.modbusView3);
            this.Controls.Add(this.modbusView2);
            this.Controls.Add(this.modbusView1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.MainFormStatusStrip);
            this.Controls.Add(this.MainFormMenuStrip);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.MainMenuStrip = this.MainFormMenuStrip;
            this.Name = "SerialportSampleForm";
            this.Text = "Serial tool Sample";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SerialportSampleForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MainFormStatusStrip.ResumeLayout(false);
            this.MainFormStatusStrip.PerformLayout();
            this.MainFormMenuStrip.ResumeLayout(false);
            this.MainFormMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.StatusStrip MainFormStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private TextBoxModbus modbusView1;
        private TextBoxModbus modbusView2;
        private TextBoxModbus modbusView3;
        private TextBoxModbus modbusView4;
        private TextBoxModbus modbusView5;
        private Custom_Controls.BitStatus bitStatus1;
        private Custom_Controls.BitStatus bitStatus2;
        private ButtonModbus buttonModbus1;
        private System.Windows.Forms.MenuStrip MainFormMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 通讯设置ToolStripMenuItem;
    }
}

