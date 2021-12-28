using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ai
{
    public class background
    {
        public int XD, YD;
        public Bitmap im;
        public int XS, YS;
    }

    public partial class Form1 : Form
    {
        Bitmap off;
        List<background> l = new List<background>();


        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Load += Form1_Load;
            this.Paint += Form1_Paint1;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void Form1_Paint1(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
            for (int i = 0; i < 2; i++)
            {
                background pnn = new background();
                pnn.XD = 0;
                pnn.YD = 0;
                pnn.XS = 0;
                pnn.YS = 0;
                if (i == 1)
                {
                    pnn.XD = Width - 20;
                }
                pnn.im = new Bitmap("Background.Wood.png");
                l.Add(pnn);
            }
        }

        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }

        void DrawScene(Graphics g)
        {
            g.Clear(Color.White);
            for (int i = 0; i < l.Count; i++)
            {
                Rectangle rcDest = new Rectangle(l[i].XD, l[i].YD, Width, Height);
                Rectangle rcSrc = new Rectangle(l[i].XS, l[i].YS, l[i].im.Width, l[i].im.Height);
                g.DrawImage(l[i].im, rcDest, rcSrc, GraphicsUnit.Pixel);
            }
        }
    }
}