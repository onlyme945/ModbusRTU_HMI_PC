using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace ModbusLib
{

    [ToolboxBitmap(typeof(TextBox))]
    public class ModbusView : System.Windows.Forms.TextBox
    {
        private TextBox textBox1;

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 0;
            this.ResumeLayout(false);

        }
    }
}
