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
            this.comboPortName = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBaudrate = new System.Windows.Forms.ComboBox();
            this.buttonOpenClose = new System.Windows.Forms.Button();
            this.labelGetCount = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txGet = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.modbusView4 = new SerialportSample.ModbusView();
            this.modbusView3 = new SerialportSample.ModbusView();
            this.modbusView1 = new SerialportSample.ModbusView();
            this.groupBox1.SuspendLayout();
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
            // labelGetCount
            // 
            this.labelGetCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelGetCount.AutoSize = true;
            this.labelGetCount.Font = new System.Drawing.Font("宋体", 9F);
            this.labelGetCount.Location = new System.Drawing.Point(476, 38);
            this.labelGetCount.Name = "labelGetCount";
            this.labelGetCount.Size = new System.Drawing.Size(35, 12);
            this.labelGetCount.TabIndex = 9;
            this.labelGetCount.Text = "Get:0";
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
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.txGet);
            this.groupBox1.Location = new System.Drawing.Point(15, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(546, 303);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data received ";
            // 
            // txGet
            // 
            this.txGet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txGet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txGet.Location = new System.Drawing.Point(7, 20);
            this.txGet.Multiline = true;
            this.txGet.Name = "txGet";
            this.txGet.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txGet.Size = new System.Drawing.Size(533, 277);
            this.txGet.TabIndex = 0;
            // 
            // statusStrip1
            // 
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
            // modbusView4
            // 
            this.modbusView4.EnablePeriodRequest = true;
            this.modbusView4.Location = new System.Drawing.Point(123, 408);
            this.modbusView4.MaxValue = 100D;
            this.modbusView4.MinValue = 1D;
            this.modbusView4.Name = "modbusView4";
            this.modbusView4.ReadAddress = ((ushort)(0));
            this.modbusView4.ReadDataLengthInWord = ((byte)(1));
            this.modbusView4.ReadDataType = SerialportSample.ModbusView.ReadDataTypeEnum.UINT16;
            this.modbusView4.ReadFuncCode = SerialportSample.ModbusView.ReadFunctionCodeEnum.ReadInputRegs;
            this.modbusView4.RequestPeriod = 100D;
            this.modbusView4.Size = new System.Drawing.Size(100, 21);
            this.modbusView4.StationID = ((byte)(1));
            this.modbusView4.TabIndex = 16;
            this.modbusView4.WriteAddress = ((ushort)(1));
            this.modbusView4.WriteDataLengthInWord = ((byte)(1));
            this.modbusView4.WriteDataType = SerialportSample.ModbusView.WriteDataTypeEnum.UINT16;
            this.modbusView4.WriteFuncCode = SerialportSample.ModbusView.WriteFunctionCodeEnum.WriteCoils;
            // 
            // modbusView3
            // 
            this.modbusView3.EnablePeriodRequest = true;
            this.modbusView3.Location = new System.Drawing.Point(264, 408);
            this.modbusView3.MaxValue = 100D;
            this.modbusView3.MinValue = 1D;
            this.modbusView3.Name = "modbusView3";
            this.modbusView3.ReadAddress = ((ushort)(1));
            this.modbusView3.ReadDataLengthInWord = ((byte)(1));
            this.modbusView3.ReadDataType = SerialportSample.ModbusView.ReadDataTypeEnum.UINT16;
            this.modbusView3.ReadFuncCode = SerialportSample.ModbusView.ReadFunctionCodeEnum.ReadInputRegs;
            this.modbusView3.RequestPeriod = 100D;
            this.modbusView3.Size = new System.Drawing.Size(100, 21);
            this.modbusView3.StationID = ((byte)(1));
            this.modbusView3.TabIndex = 15;
            this.modbusView3.WriteAddress = ((ushort)(1));
            this.modbusView3.WriteDataLengthInWord = ((byte)(1));
            this.modbusView3.WriteDataType = SerialportSample.ModbusView.WriteDataTypeEnum.UINT16;
            this.modbusView3.WriteFuncCode = SerialportSample.ModbusView.WriteFunctionCodeEnum.WriteCoils;
            // 
            // modbusView1
            // 
            this.modbusView1.EnablePeriodRequest = true;
            this.modbusView1.Location = new System.Drawing.Point(123, 366);
            this.modbusView1.MaxValue = 100D;
            this.modbusView1.MinValue = 1D;
            this.modbusView1.Name = "modbusView1";
            this.modbusView1.ReadAddress = ((ushort)(2));
            this.modbusView1.ReadDataLengthInWord = ((byte)(2));
            this.modbusView1.ReadDataType = SerialportSample.ModbusView.ReadDataTypeEnum.FLOAT32;
            this.modbusView1.ReadFuncCode = SerialportSample.ModbusView.ReadFunctionCodeEnum.ReadInputRegs;
            this.modbusView1.RequestPeriod = 100D;
            this.modbusView1.Size = new System.Drawing.Size(100, 21);
            this.modbusView1.StationID = ((byte)(1));
            this.modbusView1.TabIndex = 13;
            this.modbusView1.WriteAddress = ((ushort)(1));
            this.modbusView1.WriteDataLengthInWord = ((byte)(1));
            this.modbusView1.WriteDataType = SerialportSample.ModbusView.WriteDataTypeEnum.UINT16;
            this.modbusView1.WriteFuncCode = SerialportSample.ModbusView.WriteFunctionCodeEnum.WriteCoils;
            // 
            // SerialportSampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 468);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.modbusView4);
            this.Controls.Add(this.modbusView3);
            this.Controls.Add(this.modbusView1);
            this.Controls.Add(this.labelGetCount);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonOpenClose);
            this.Controls.Add(this.comboBaudrate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboPortName);
            this.Name = "SerialportSampleForm";
            this.Text = "Serial tool Sample";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.Label labelGetCount;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txGet;
        private ModbusView modbusView1;
        private ModbusView modbusView3;
        private ModbusView modbusView4;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}

