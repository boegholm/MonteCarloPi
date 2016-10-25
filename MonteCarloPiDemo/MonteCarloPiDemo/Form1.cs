using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MonteCarloPiDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int count = 0;
            if (int.TryParse(textBox1.Text, out count))
            {
                backgroundWorker1.RunWorkerAsync(count);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int highestPercentageReached = -1;
            int? count = e.Argument as int?;
            if (count != null)
            {
                for (int i = 0; i < count; i++)
                {
                    circleBox1.AddRandomPoint();
                    int percentComplete = (int)((float)i / (float)count * 100);
                    if (percentComplete > highestPercentageReached)
                    {
                        highestPercentageReached = percentComplete;
                        backgroundWorker1.ReportProgress(percentComplete);
                    }
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            BeginInvoke((Action)(() =>
            {
                label1.Text = $"PI: {circleBox1.Pi:0.######}";
                double accuracy = 1.0 - Math.Abs(circleBox1.Pi / Math.PI - 1.0);
                label2.Text = $"Accuracy {(accuracy) * 100.0:0.####}%";
            }));
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            circleBox1.DotSize = trackBar1.Value;
        }
    }
}
