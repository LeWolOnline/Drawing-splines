using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Grafic1
{
    public partial class Form1 : Form
    {
        Point[] PointM = new Point [100];
        Point PNT = new Point();
        Point Check = new Point();      
        int Mass = 0; 
        const double T = 0.001; //Точность прорисовки сплайнов

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void PictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.MouseClick += OnPictureBoxClicked;
        }
        void OnPictureBoxClicked(object sender, MouseEventArgs args)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.SmoothingMode = SmoothingMode.HighQuality;
            var location = args.Location;
            PNT = location;
            if (Check != PNT) {
                Check = PNT;
                PointM[Mass] = PNT;
                textBox2.Text += (Mass + 1) + ") " + PointM[Mass] + Environment.NewLine;
                Mass++;
            }
            DrawPoint();
        }

        /*******************ОБРАБОТКА КНОПОК*******************/
        private void Button1_Click(object sender, EventArgs e) // Очистить всё
        {
            textBox2.Text = "";
            Mass = 0;
            DrawClear();
        }
        private void Button4_Click(object sender, EventArgs e) // Построение сплайна Безье
        {
            DrawClear();
            DrawPoint();
            DrawLine();
            DrawBezier();
        }
        private void Button6_Click(object sender, EventArgs e) // Построение кривой Эрмита
        {
            DrawClear();
            DrawPoint();
            DrawLine();
            DrawErmit();
        }
        private void Button7_Click(object sender, EventArgs e) // Построение В-сплайна
        {
            DrawClear();
            DrawPoint();
            DrawLine();
            DrawBSplain();
        }
        private void Button8_Click(object sender, EventArgs e) // Построение Catmull-Rom
        {
            DrawClear();
            DrawPoint();
            DrawLine();
            DrawCatmull();
        }
        private void Button5_Click(object sender, EventArgs e) // Очистить построения
        {
            DrawClear();
            for (int i = 0; i < Mass; i++)
            {
                PNT = PointM[i];
                DrawPoint();
            }
        }

        /*******************РИСОВАНИЕ*******************/
        private void DrawLine () 
        {
            Graphics g = pictureBox1.CreateGraphics();
            Pen penGray = new Pen(Brushes.Gray, 1);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            for (int i = 0; i < Mass - 1; i++)
            {
                g.DrawLine(penGray, PointM[i], PointM[i + 1]);
            }
        }
        private void DrawPoint() 
        {

            Graphics g = pictureBox1.CreateGraphics();
            Brush aBrush = (Brush)Brushes.Black;
            for (int i = 0; i < Mass; i++)
            {
                g.DrawEllipse(new Pen(Brushes.Black, 3), PointM[i].X - 1, PointM[i].Y - 1, 2, 2);
            }
        }
        private void DrawBezier ()
        {
            for (int p = 0; p < Mass - 3; p += 3)
            {
                for (double i = 0; i < 1; i += T)
                {
                    Point R = new Point();
                    double RX, RY;
                    RX = Math.Pow(1 - i, 3) * PointM[p].X + 3 * i * Math.Pow(1 - i, 2) * PointM[p+1].X + 3 * Math.Pow(i, 2) * (1 - i) * PointM[p+2].X + Math.Pow(i, 3) * PointM[p+3].X;
                    RY = Math.Pow(1 - i, 3) * PointM[p].Y + 3 * i * Math.Pow(1 - i, 2) * PointM[p+1].Y + 3 * Math.Pow(i, 2) * (1 - i) * PointM[p+2].Y + Math.Pow(i, 3) * PointM[p+3].Y;
                    R.X = (int)RX;
                    R.Y = (int)RY;
                    Graphics g = pictureBox1.CreateGraphics();
                    Brush aBrush = (Brush)Brushes.Black;
                    g.FillRectangle(aBrush, R.X - 1, R.Y - 1, 2, 2);
                }
            }
        }
        private void DrawErmit()
        {
            for (int p = 0; p < Mass - 3; p += 2)
            {
                for (double i = 0; i < 1; i += T)
                {
                    Point R = new Point();
                    double RX, RY;
                    double c0 = 1 - 3 * i * i + 2 * i * i * i;
                    double c1 = i - 2 * i * i + i * i * i;
                    double c2 = 3 * i * i - 2 * i * i * i;
                    double c3 = -i * i + i * i * i;
                    RY = c0 * PointM[p].Y + c1 * (PointM[p + 1].Y - PointM[p].Y) + c2 * PointM[p + 2].Y + c3 * (PointM[p + 3].Y - PointM[p + 2].Y);
                    RX = c0 * PointM[p].X + c1 * (PointM[p + 1].X - PointM[p].X) + c2 * PointM[p + 2].X + c3 * (PointM[p + 3].X - PointM[p + 2].X);
                    R.X = (int)RX;
                    R.Y = (int)RY;
                    Graphics g = pictureBox1.CreateGraphics();
                    Brush aBrush = (Brush)Brushes.Black;
                    g.FillRectangle(aBrush, R.X - 1, R.Y - 1, 2, 2);
                }
            }
        }
        private void DrawBSplain()
        {

            for (int p = 0; p < Mass - 3; p += 1)
            {
                for (double i = 0; i < 1; i += T)
                {
                    Point R = new Point();
                    double RX, RY;
                    RX = Math.Pow(1 - i, 3) / 6 * PointM[p].X + (3 * Math.Pow(i, 3) - 6 * Math.Pow(i, 2) + 4) / 6 * PointM[p+1].X + (-3 * Math.Pow(i, 3) + 3 * Math.Pow(i, 2) + 3 * i + 1) / 6 * PointM[p+2].X + Math.Pow(i, 3) / 6 * PointM[p+3].X;
                    RY = Math.Pow(1 - i, 3) / 6 * PointM[p].Y + (3 * Math.Pow(i, 3) - 6 * Math.Pow(i, 2) + 4) / 6 * PointM[p+1].Y + (-3 * Math.Pow(i, 3) + 3 * Math.Pow(i, 2) + 3 * i + 1) / 6 * PointM[p+2].Y + Math.Pow(i, 3) / 6 * PointM[p+3].Y;
                    R.X = (int)RX;
                    R.Y = (int)RY;
                    Graphics g = pictureBox1.CreateGraphics();
                    Brush aBrush = (Brush)Brushes.Black;
                    g.FillRectangle(aBrush, R.X - 1, R.Y - 1, 2, 2);
                }
            }
        }
        private void DrawCatmull()
        {
            for (int p = 0; p < Mass - 3; p += 1)
            {
                for (double i = 0; i < 1; i += T)
                {
                    Point R = new Point();
                    double RX, RY;
                    RX = (-i * (1 - i) * (1 - i) * PointM[p].X + (2 - 5 * i * i + 3 * i * i * i) * PointM[p+1].X + i * (1 + 4 * i - 3 * i * i) * PointM[p+2].X - i * i * (1 - i) * PointM[p+3].X) / 2;
                    RY = (-i * (1 - i) * (1 - i) * PointM[p].Y + (2 - 5 * i * i + 3 * i * i * i) * PointM[p+1].Y + i * (1 + 4 * i - 3 * i * i) * PointM[p+2].Y - i * i * (1 - i) * PointM[p+3].Y) / 2;
                    R.X = (int)RX;
                    R.Y = (int)RY;

                    Graphics g = pictureBox1.CreateGraphics();
                    Brush aBrush = (Brush)Brushes.Black;
                    g.FillRectangle(aBrush, R.X - 1, R.Y - 1, 2, 2);
                }
            }
        }
        private void DrawClear()
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.Clear(Color.White);
        }
    }
}
