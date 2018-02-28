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
            this.modbusView8 = new SerialportSample.ModbusView();
            this.modbusView4 = new SerialportSample.ModbusView();
            this.modbusView7 = new SerialportSample.ModbusView();
            this.modbusView3 = new SerialportSample.ModbusView();
            this.modbusView6 = new SerialportSample.ModbusView();
            this.modbusView1 = new SerialportSample.ModbusView();
            this.modbusView5 = new SerialportSample.ModbusView();
            this.modbusView2 = new SerialportSample.ModbusView();
            this.groupBox1.SuspendLayout();
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
            // modbusView8
            // 
            this.modbusView8.EnablePeriodRequest = true;
            this.modbusView8.Location = new System.Drawing.Point(403, 412);
            this.modbusView8.MaxValue = 100D;
            this.modbusView8.MinValue = 1D;
            this.modbusView8.Name = "modbusView8";
            this.modbusView8.ReadAddress = ((ushort)(6));
            this.modbusView8.ReadFuncCode = SerialportSample.ModbusView.ReadFunctionCodeEnum.ReadInputRegs;
            this.modbusView8.RequestPeriod = 1000D;
            this.modbusView8.Size = new System.Drawing.Size(100, 21);
            this.modbusView8.StationID = ((byte)(1));
            this.modbusView8.TabIndex = 3;
            this.modbusView8.Text = "1";
            this.modbusView8.WriteAddress = ((ushort)(4));
            this.modbusView8.WriteFuncCode = SerialportSample.ModbusView.WriteFunctionCodeEnum.WriteCoils;
            // 
            // modbusView4
            // 
            this.modbusView4.EnablePeriodRequest = true;
            this.modbusView4.Location = new System.Drawing.Point(403, 385);
            this.modbusView4.MaxValue = 100D;
            this.modbusView4.MinValue = 1D;
            this.modbusView4.Name = "modbusView4";
            this.modbusView4.ReadAddress = ((ushort)(6));
            this.modbusView4.ReadFuncCode = SerialportSample.ModbusView.ReadFunctionCodeEnum.ReadInputRegs;
            this.modbusView4.RequestPeriod = 1000D;
            this.modbusView4.Size = new System.Drawing.Size(100, 21);
            this.modbusView4.StationID = ((byte)(1));
            this.modbusView4.TabIndex = 3;
            this.modbusView4.Text = "1";
            this.modbusView4.WriteAddress = ((ushort)(4));
            this.modbusView4.WriteFuncCode = SerialportSample.ModbusView.WriteFunctionCodeEnum.WriteCoils;
            // 
            // modbusView7
            // 
            this.modbusView7.EnablePeriodRequest = true;
            this.modbusView7.Location = new System.Drawing.Point(297, 412);
            this.modbusView7.MaxValue = 100D;
            this.modbusView7.MinValue = 1D;
            this.modbusView7.Name = "modbusView7";
            this.modbusView7.ReadAddress = ((ushort)(6));
            this.modbusView7.ReadFuncCode = SerialportSample.ModbusView.ReadFunctionCodeEnum.ReadInputRegs;
            this.modbusView7.RequestPeriod = 1000D;
            this.modbusView7.Size = new System.Drawing.Size(100, 21);
            this.modbusView7.StationID = ((byte)(1));
            this.modbusView7.TabIndex = 3;
            this.modbusView7.Text = "1";
            this.modbusView7.WriteAddress = ((ushort)(4));
            this.modbusView7.WriteFuncCode = SerialportSample.ModbusView.WriteFunctionCodeEnum.WriteCoils;
            // 
            // modbusView3
            // 
            this.modbusView3.EnablePeriodRequest = true;
            this.modbusView3.Location = new System.Drawing.Point(297, 385);
            this.modbusView3.MaxValue = 100D;
            this.modbusView3.MinValue = 1D;
            this.modbusView3.Name = "modbusView3";
            this.modbusView3.ReadAddress = ((ushort)(6));
            this.modbusView3.ReadFuncCode = SerialportSample.ModbusView.ReadFunctionCodeEnum.ReadInputRegs;
            this.modbusView3.RequestPeriod = 1000D;
            this.modbusView3.Size = new System.Drawing.Size(100, 21);
            this.modbusView3.StationID = ((byte)(1));
            this.modbusView3.TabIndex = 3;
            this.modbusView3.Text = "1";
            this.modbusView3.WriteAddress = ((ushort)(4));
            this.modbusView3.WriteFuncCode = SerialportSample.ModbusView.WriteFunctionCodeEnum.WriteCoils;
            // 
            // modbusView6
            // 
            this.modbusView6.EnablePeriodRequest = true;
            this.modbusView6.Location = new System.Drawing.Point(170, 412);
            this.modbusView6.MaxValue = 100D;
            this.modbusView6.MinValue = 1D;
            this.modbusView6.Name = "modbusView6";
            this.modbusView6.ReadAddress = ((ushort)(6));
            this.modbusView6.ReadFuncCode = SerialportSample.ModbusView.ReadFunctionCodeEnum.ReadInputRegs;
            this.modbusView6.RequestPeriod = 1000D;
            this.modbusView6.Size = new System.Drawing.Size(100, 21);
            this.modbusView6.StationID = ((byte)(1));
            this.modbusView6.TabIndex = 3;
            this.modbusView6.Text = "1";
            this.modbusView6.WriteAddress = ((ushort)(4));
            this.modbusView6.WriteFuncCode = SerialportSample.ModbusView.WriteFunctionCodeEnum.WriteCoils;
            // 
            // modbusView1
            // 
            this.modbusView1.EnablePeriodRequest = true;
            this.modbusView1.Location = new System.Drawing.Point(170, 385);
            this.modbusView1.MaxValue = 100D;
            this.modbusView1.MinValue = 1D;
            this.modbusView1.Name = "modbusView1";
            this.modbusView1.ReadAddress = ((ushort)(6));
            this.modbusView1.ReadFuncCode = SerialportSample.ModbusView.ReadFunctionCodeEnum.ReadInputRegs;
            this.modbusView1.RequestPeriod = 1000D;
            this.modbusView1.Size = new System.Drawing.Size(100, 21);
            this.modbusView1.StationID = ((byte)(1));
            this.modbusView1.TabIndex = 3;
            this.modbusView1.Text = "1";
            this.modbusView1.WriteAddress = ((ushort)(4));
            this.modbusView1.WriteFuncCode = SerialportSample.ModbusView.WriteFunctionCodeEnum.WriteCoils;
            // 
            // modbusView5
            // 
            this.modbusView5.EnablePeriodRequest = true;
            this.modbusView5.Location = new System.Drawing.Point(59, 412);
            this.modbusView5.MaxValue = 100D;
            this.modbusView5.MinValue = 1D;
            this.modbusView5.Name = "modbusView5";
            this.modbusView5.ReadAddress = ((ushort)(6));
            this.modbusView5.ReadFuncCode = SerialportSample.ModbusView.ReadFunctionCodeEnum.ReadInputRegs;
            this.modbusView5.RequestPeriod = 1000D;
            this.modbusView5.Size = new System.Drawing.Size(100, 21);
            this.modbusView5.StationID = ((byte)(1));
            this.modbusView5.TabIndex = 3;
            this.modbusView5.Text = "1";
            this.modbusView5.WriteAddress = ((ushort)(4));
            this.modbusView5.WriteFuncCode = SerialportSample.ModbusView.WriteFunctionCodeEnum.WriteCoils;
            // 
            // modbusView2
            // 
            this.modbusView2.EnablePeriodRequest = true;
            this.modbusView2.Location = new System.Drawing.Point(59, 385);
            this.modbusView2.MaxValue = 100D;
            this.modbusView2.MinValue = 1D;
            this.modbusView2.Name = "modbusView2";
            this.modbusView2.ReadAddress = ((ushort)(6));
            this.modbusView2.ReadFuncCode = SerialportSample.ModbusView.ReadFunctionCodeEnum.ReadInputRegs;
            this.modbusView2.RequestPeriod = 1000D;
            this.modbusView2.Size = new System.Drawing.Size(100, 21);
            this.modbusView2.StationID = ((byte)(1));
            this.modbusView2.TabIndex = 3;
            this.modbusView2.Text = "1";
            this.modbusView2.WriteAddress = ((ushort)(4));
            this.modbusView2.WriteFuncCode = SerialportSample.ModbusView.WriteFunctionCodeEnum.WriteCoils;
            // 
            // SerialportSampleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 468);
            this.Controls.Add(this.modbusView8);
            this.Controls.Add(this.labelGetCount);
            this.Controls.Add(this.modbusView4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.modbusView7);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.modbusView3);
            this.Controls.Add(this.buttonOpenClose);
            this.Controls.Add(this.modbusView6);
            this.Controls.Add(this.comboBaudrate);
            this.Controls.Add(this.modbusView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.modbusView5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.modbusView2);
            this.Controls.Add(this.comboPortName);
            this.Name = "SerialportSampleForm";
            this.Text = "Serial tool Sample";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private ModbusView modbusView2;
        private ModbusView modbusView8;
        private ModbusView modbusView4;
        private ModbusView modbusView7;
        private ModbusView modbusView3;
        private ModbusView modbusView6;
        private ModbusView modbusView1;
        private ModbusView modbusView5;
    }
}

