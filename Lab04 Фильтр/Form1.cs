using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab04_Фильтр
{
    public partial class Form1 : Form
    {
        Image tempbw;
        Image tempLinear;
        Image tempMasked;


        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            pictureBox1.Load(".\\" + comboBox1.SelectedItem.ToString());

            tempbw = ToBW((Image)pictureBox1.Image.Clone());
            pictureBox2.Image = tempbw;

            tempLinear = Linear((Image)pictureBox2.Image.Clone());
            pictureBox3.Image = tempLinear;

            tempMasked = Masked((Image)pictureBox2.Image.Clone());
            pictureBox4.Image = tempMasked;
        }

        
        static Bitmap ToBW(Image image)
        {
            Bitmap bmp = new Bitmap(image);
            Color c;
            int grayScale;
            for (int i = 0; i < bmp.Height; i++){
                for (int j = 0; j < bmp.Width; j++) {
                    c = bmp.GetPixel(j, i);
                    grayScale = (int)((c.R * .3) + (c.G * .59) + (c.B * .11));
                    c = Color.FromArgb(grayScale,grayScale,grayScale);
                    bmp.SetPixel(j, i, c);
                }
            }
            return bmp;
        }


        static Bitmap Linear(Image image)
        {
            Bitmap bmp = new Bitmap(image);
            int xmin = 255;
            int xmax = 0;
            int x;

            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    if ((x = bmp.GetPixel(j, i).R) < xmin) xmin = x;

                    if ((x = bmp.GetPixel(j, i).R) > xmax) xmax = x;
                }
            }

            Color c;

            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    c = bmp.GetPixel(j, i);
                    c = Color.FromArgb((c.R-xmin)  * 255 / (xmax - xmin), ((c.G - xmin) * 255 ) / (xmax - xmin), ((c.B - xmin) * 255 / (xmax - xmin)));
                    bmp.SetPixel(j, i, c);
                }
            }

            return bmp;
        }

        static Bitmap Masked(Image image)
        {
            Bitmap bmp1 = new Bitmap(image);
            Bitmap bmp = new Bitmap(bmp1.Width+2, bmp1.Height+2);
            int mod=0;

            for (int i = 0; i < bmp1.Height; i++)
            {
                for (int j = 0; j < bmp1.Width; j++)
                {
                    bmp.SetPixel(j + 1, i + 1, bmp1.GetPixel(j, i));
                }
            }
           
            for (int j = 0; j < bmp1.Width; j++)
            {

                bmp.SetPixel(j, 0, bmp1.GetPixel(j, 0));
                bmp.SetPixel(j, bmp.Height-1, bmp1.GetPixel(j, bmp1.Height-1));
            }

            for (int j = 0; j < bmp1.Height; j++)
            {

                bmp.SetPixel(0, j, bmp1.GetPixel(j, 0));
                bmp.SetPixel(bmp.Width - 1, j, bmp1.GetPixel(bmp1.Width - 1, j));
            }

            bmp.SetPixel(0, 0, bmp1.GetPixel(0, 0));
            bmp.SetPixel(0, bmp.Height - 1, bmp1.GetPixel(0, bmp1.Height - 1));
            bmp.SetPixel(bmp.Width - 1, bmp.Height - 1, bmp1.GetPixel(bmp1.Width - 1, bmp1.Height - 1));
            bmp.SetPixel(bmp.Width - 1, 0, bmp1.GetPixel(bmp1.Width - 1, 0));

            for (int i = 0; i < bmp1.Height; i++)
            {
                for (int j = 0; j < bmp1.Width; j++)
                {

                    mod = 0;
                    for (int y = -1; y < 2; y++)
                        for (int x = -1; x < 2; x++)
                        {
                            mod += bmp.GetPixel(j + 1 + x, i + 1 + y).R;
                        }
                    mod /= 9;
                    bmp1.SetPixel(j, i, Color.FromArgb(mod, mod, mod));
                }
            }
            return bmp1;
        }

            private void button1_Click(object sender, EventArgs e)
        {
            tempbw.Save(".\\tempbw.bmp");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tempLinear.Save(".\\linear.bmp");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tempMasked.Save(".\\masked.bmp");
        }

       
    }
}
