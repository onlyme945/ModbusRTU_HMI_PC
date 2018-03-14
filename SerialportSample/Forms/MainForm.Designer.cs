﻿namespace SerialportSample
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
            this.comboPortName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBaudrate = new System.Windows.Forms.ComboBox();
            this.buttonOpenClose = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.buttonModbus1 = new SerialportSample.ButtonModbus();
            this.labelModbus1 = new SerialportSample.LabelModbus();
            this.modbusView5 = new SerialportSample.TextBoxModbus();
            this.modbusView4 = new SerialportSample.TextBoxModbus();
            this.modbusView3 = new SerialportSample.TextBoxModbus();
            this.modbusView2 = new SerialportSample.TextBoxModbus();
            this.modbusView1 = new SerialportSample.TextBoxModbus();
            this.labelModbus2 = new SerialportSample.LabelModbus();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboPortName
            // 
            this.comboPortName.BackColor = System.Drawing.Color.White;
            this.comboPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboPortName.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboPortName.FormattingEnabled = true;
            this.comboPortName.Location = new System.Drawing.Point(78, 11);
            this.comboPortName.Name = "comboPortName";
            this.comboPortName.Size = new System.Drawing.Size(121, 20);
            this.comboPortName.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Port name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(205, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Baudrate";
            // 
            // comboBaudrate
            // 
            this.comboBaudrate.BackColor = System.Drawing.Color.White;
            this.comboBaudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBaudrate.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBaudrate.FormattingEnabled = true;
            this.comboBaudrate.Items.AddRange(new object[] {
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.comboBaudrate.Location = new System.Drawing.Point(264, 11);
            this.comboBaudrate.Name = "comboBaudrate";
            this.comboBaudrate.Size = new System.Drawing.Size(121, 20);
            this.comboBaudrate.TabIndex = 5;
            // 
            // buttonOpenClose
            // 
            this.buttonOpenClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonOpenClose.Font = new System.Drawing.Font("宋体", 9F);
            this.buttonOpenClose.ForeColor = System.Drawing.Color.Black;
            this.buttonOpenClose.Location = new System.Drawing.Point(405, 10);
            this.buttonOpenClose.Name = "buttonOpenClose";
            this.buttonOpenClose.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenClose.TabIndex = 0;
            this.buttonOpenClose.Text = "Open";
            this.buttonOpenClose.UseVisualStyleBackColor = true;
            this.buttonOpenClose.Click += new System.EventHandler(this.buttonOpenClose_Click);
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonReset.Font = new System.Drawing.Font("宋体", 9F);
            this.buttonReset.ForeColor = System.Drawing.Color.Black;
            this.buttonReset.Location = new System.Drawing.Point(486, 10);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 1;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 446);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(573, 22);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(436, 209);
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
            // buttonModbus1
            // 
            this.buttonModbus1.Location = new System.Drawing.Point(436, 147);
            this.buttonModbus1.Name = "buttonModbus1";
            this.buttonModbus1.Size = new System.Drawing.Size(81, 29);
            this.buttonModbus1.TabIndex = 53;
            // 
            // labelModbus1
            // 
            this.labelModbus1.AutoSize = true;
            this.labelModbus1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelModbus1.EnablePeriodRequest = true;
            this.labelModbus1.Location = new System.Drawing.Point(240, 110);
            this.labelModbus1.Name = "labelModbus1";
            this.labelModbus1.ReadAddress = ((ushort)(0));
            this.labelModbus1.ReadDataLengthInWord = ((byte)(1));
            this.labelModbus1.ReadDataType = SerialportSample.LabelModbus.ReadDataTypeEnum.UINT16;
            this.labelModbus1.ReadFuncCode = SerialportSample.LabelModbus.ReadFunctionCodeEnum.ReadStorageRegs;
            this.labelModbus1.RequestPeriod = 500D;
            this.labelModbus1.Size = new System.Drawing.Size(13, 14);
            this.labelModbus1.StationID = ((byte)(1));
            this.labelModbus1.TabIndex = 52;
            this.labelModbus1.Text = "0";
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
            // labelModbus2
            // 
            this.labelModbus2.AutoSize = true;
            this.labelModbus2.EnablePeriodRequest = true;
            this.labelModbus2.Location = new System.Drawing.Point(238, 164);
            this.labelModbus2.Name = "labelModbus2";
            this.labelModbus2.ReadAddress = ((ushort)(1));
            this.labelModbus2.ReadDataLengthInWord = ((byte)(1));
            this.labelModbus2.ReadDataType = SerialportSample.LabelModbus.ReadDataTypeEnum.UINT16;
            this.labelModbus2.ReadFuncCode = SerialportSample.LabelModbus.ReadFunctionCodeEnum.ReadStorageRegs;
            this.labelModbus2.RequestPeriod = 100D;
            this.labelModbus2.Size = new System.Drawing.Size(11, 12);
            this.labelModbus2.StationID = ((byte)(1));
            this.labelModbus2.TabIndex = 54;
            this.labelModbus2.Text = "0";
            // 
            // SerialportSampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 468);
            this.Controls.Add(this.labelModbus2);
            this.Controls.Add(this.buttonModbus1);
            this.Controls.Add(this.labelModbus1);
            this.Controls.Add(this.modbusView5);
            this.Controls.Add(this.modbusView4);
            this.Controls.Add(this.modbusView3);
            this.Controls.Add(this.modbusView2);
            this.Controls.Add(this.modbusView1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonOpenClose);
            this.Controls.Add(this.comboBaudrate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboPortName);
            this.Name = "SerialportSampleForm";
            this.Text = "Serial tool Sample";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboPortName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBaudrate;
        private System.Windows.Forms.Button buttonOpenClose;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private TextBoxModbus modbusView1;
        private TextBoxModbus modbusView2;
        private TextBoxModbus modbusView3;
        private TextBoxModbus modbusView4;
        private TextBoxModbus modbusView5;
        private LabelModbus labelModbus1;
        private ButtonModbus buttonModbus1;
        private LabelModbus labelModbus2;
    }
}
