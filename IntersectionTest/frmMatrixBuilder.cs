using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IntersectionTest
{
    public partial class frmMatrixBuilder : Form
    {
        public frmMatrixBuilder()
        {
            InitializeComponent();
        }

        private void cmdSelectFolder_Click(object sender, EventArgs e)
        {
            if (fb.ShowDialog() == DialogResult.OK)
            {
                txtFolder.Text = fb.SelectedPath;
            }
        }

        private MatrixBuilder mb;

        private void button1_Click(object sender, EventArgs e)
        {
            mb = new MatrixBuilder(txtCN.Text);
         
            mb.BuildDaylyMatrix(txtFolder.Text + "\\Dayly.xlsx");
        
            MessageBox.Show("Done!");
        }

      
        private void button2_Click(object sender, EventArgs e)
        {
            mb = new MatrixBuilder(txtCN.Text);
         
            mb.BuildWinterMatrix(txtFolder.Text + "\\Winter_all.xlsx",0);
        
            MessageBox.Show("Done!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mb = new MatrixBuilder(txtCN.Text);
         
            mb.BuildSummerMatrix(txtFolder.Text + "\\Summer_all.xlsx",0);
        
            MessageBox.Show("Done!");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            mb = new MatrixBuilder(txtCN.Text);
         
            mb.BuildWinterMatrix(txtFolder.Text + "\\Winter_W.xlsx", 1);
        
            MessageBox.Show("Done!");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            mb = new MatrixBuilder(txtCN.Text);
         
            mb.BuildWinterMatrix(txtFolder.Text + "\\Winter_H.xlsx", 2);
        
            MessageBox.Show("Done!");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            mb = new MatrixBuilder(txtCN.Text);
         
            mb.BuildSummerMatrix(txtFolder.Text + "\\Summer_W.xlsx", 1);
        
            MessageBox.Show("Done!");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            mb = new MatrixBuilder(txtCN.Text);
         
            mb.BuildSummerMatrix(txtFolder.Text + "\\Summer_H.xlsx", 2);
        
            MessageBox.Show("Done!");
        }

        private void frmMatrixBuilder_Load(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            mb = new MatrixBuilder(txtCN.Text);

            mb.BuildWinterMatrix(txtFolder.Text + "\\TimeWinter_all.xlsx", 5);

            MessageBox.Show("Done!");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            mb = new MatrixBuilder(txtCN.Text);

            mb.BuildWinterMatrix(txtFolder.Text + "\\TimeWinter_H.xlsx", 4);

            MessageBox.Show("Done!");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            mb = new MatrixBuilder(txtCN.Text);

            mb.BuildWinterMatrix(txtFolder.Text + "\\TimeWinter_W.xlsx", 3);

            MessageBox.Show("Done!");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            mb = new MatrixBuilder(txtCN.Text);

            mb.BuildSummerMatrix(txtFolder.Text + "\\TimeSummer_all.xlsx", 5);

            MessageBox.Show("Done!");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            mb = new MatrixBuilder(txtCN.Text);

            mb.BuildSummerMatrix(txtFolder.Text + "\\TimeSummer_W.xlsx", 3);

            MessageBox.Show("Done!");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            mb = new MatrixBuilder(txtCN.Text);

            mb.BuildSummerMatrix(txtFolder.Text + "\\TimeSummer_H.xlsx", 4);

            MessageBox.Show("Done!");
        }
    }
}
