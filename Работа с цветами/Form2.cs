using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Работа_с_цветами
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    pictureBox1.Image = new Bitmap(ofd.FileName);
                }
                catch 
                {
                    MessageBox.Show("Невозможно открыть выбранный файл", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void drawButton_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Не удалось получить картинку", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Bitmap input = new Bitmap(pictureBox1.Image);
            Bitmap output2 = new Bitmap(input.Width, input.Height);
            Bitmap output3 = new Bitmap(input.Width, input.Height);
            for (int j = 0; j < input.Height; j++)
                for (int i = 0; i < input.Width; i++)
                {
                    UInt32 pixel = (UInt32)(input.GetPixel(i, j).ToArgb());
                    float R = (float)((pixel & 0x00FF0000) >> 16); 
                    float G = (float)((pixel & 0x0000FF00) >> 8); 
                    float B = (float)(pixel & 0x000000FF); 

                    float R2, G2, B2;                                     
                    R2 = G2 = B2 = (R + G + B) / 3.0f;
                    UInt32 newPixel2 = 0xFF000000 | ((UInt32)R2 << 16) | ((UInt32)G2 << 8) | ((UInt32)B2);
                    output2.SetPixel(i, j, Color.FromArgb((int)newPixel2));

                    float R3, G3, B3;
                    R3 = G3 = B3 = (float)(0.299 * R + 0.587 * G + 0.114 * B);
                    UInt32 newPixel3 = 0xFF000000 | ((UInt32)R3 << 16) | ((UInt32)G3 << 8) | ((UInt32)B3);
                    output3.SetPixel(i, j, Color.FromArgb((int)newPixel3));
                }
            pictureBox2.Image = output2;
            pictureBox3.Image = output3;
            

            if ((pictureBox2.Image != null) && (pictureBox3.Image != null))
            {
                Bitmap first = new Bitmap(pictureBox2.Image);
                Bitmap second = new Bitmap(pictureBox3.Image);
                Bitmap output = new Bitmap(first.Width, first.Height);
                for (int j = 0; j < first.Height; j++)
                    for (int i = 0; i < first.Width; i++)
                    {
                        UInt32 pixel1 = (UInt32)(first.GetPixel(i, j).ToArgb());
                        UInt32 pixel2 = (UInt32)(second.GetPixel(i, j).ToArgb());

                        float R1 = (float)((pixel1 & 0x00FF0000) >> 16);
                        float G1 = (float)((pixel1 & 0x0000FF00) >> 8);
                        float B1 = (float)(pixel1 & 0x000000FF);
                        R1 = G1 = B1 = (R1 + G1 + B1) / 3.0f;

                        float R2 = (float)((pixel2 & 0x00FF0000) >> 16);
                        float G2 = (float)((pixel2 & 0x0000FF00) >> 8);
                        float B2 = (float)(pixel2 & 0x000000FF);
                        R2 = G2 = B2 = (float)(0.299 * R2 + 0.587 * G2 + 0.114 * B2);

                        if (R1 > R2)
                            R1 -= R2;
                        else
                            R1 = R2 - R1;
                        UInt32 newPixel1 = 0xFF000000 | ((UInt32)R1 << 16) | ((UInt32)R1 << 8) | ((UInt32)R1);
                        output.SetPixel(i, j, Color.FromArgb((int)newPixel1));
                    }
                pictureBox4.Image = output;
            }

            int[] hist = new int[256];
            if (pictureBox1.Image != null)
            {
                Bitmap source = new Bitmap(pictureBox1.Image);
                for (int j = 0; j < source.Height; j++)
                    for (int i = 0; i < source.Width; i++)
                    {
                        System.Drawing.Color c = source.GetPixel(i, j); // получаем цвет указанной точки
                        int r = Convert.ToInt32(c.R);
                        int b = Convert.ToInt32(c.B);
                        int g = Convert.ToInt32(c.G);
                        int brit = Convert.ToInt32(0.299 * r + 0.587 * g + 0.114 * b); //Перевод из RGB в полутон
                        hist[Convert.ToByte(brit)]++;
                    }
                chart1.Series[0].Points.Clear();
                for (int i = 0; i < 256; i++)
                {
                    chart1.Series[0].Points.AddXY(i, hist[i]);
                }
            }
        }

    }
}
