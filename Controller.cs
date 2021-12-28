using System;
using System.Drawing;
using System.Windows.Forms;

namespace ai
{
    public partial class Controller : Form
    {
        private Bitmap off;
        private Agent agent;
        private Player player;
        private Board board = null;
        private int columns = 7, rows = 6, playerTurn = 1;
        private bool StartGame = true, startplayer = true;

        public Controller()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Load += Form1_Load;
            this.Paint += Form1_Paint1;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (StartGame && columns < 10)
                    {
                        columns++;
                    }
                    break;
                case Keys.Down:
                    if (StartGame && columns > 5)
                    {
                        columns--;
                    }
                    break;
                case Keys.Left:
                    if (StartGame && rows < 10)
                    {
                        rows++;
                    }
                    break;
                case Keys.Right:
                    if (StartGame && rows > 5)
                    {
                        rows--;
                    }
                    break;
                case Keys.Enter:
                    StartGame = false;
                    board = new Board(rows, columns, 4);
                    break;
                case Keys.NumPad1:
                    if (!StartGame && startplayer)
                    {
                        startplayer = false;
                        playerTurn = 1;
                    }
                    break;
                case Keys.NumPad2:
                    if (!StartGame && startplayer)
                    {
                        startplayer = false;
                        playerTurn = 2;
                    }
                    break;
            }
            DrawDubb(CreateGraphics());
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
            SolidBrush redBrush = new SolidBrush(Color.Red);
            SolidBrush blueBrush = new SolidBrush(Color.Blue);
            SolidBrush yellowBrush = new SolidBrush(Color.Yellow);
            SolidBrush grayBrush = new SolidBrush(Color.Gray);
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            Font drawFont = new Font("Arial", 16);
            Pen blackPen = new Pen(Color.Black, 5);
            int x = (Width - (columns * 75)) / 2, y = 10;
            for (int i = 0; i < rows; i++)
            {
                for (int k = 0; k < columns; k++)
                {
                    g.FillRectangle(yellowBrush, x, y, 75, 75);
                    g.FillEllipse(grayBrush, x, y, 70, 70);
                    x += 75;
                }
                y += 75;
                x = (Width - (columns * 75)) / 2;
            }
            x = (Width - (columns * 75)) / 2; y = 10;
            g.DrawRectangle(blackPen, x - 3, y - 3, columns * 70 + columns * 5, rows * 70 + rows * 5);
            if (!StartGame)
            {
                g.DrawString("The Game Started:", drawFont, blackBrush, 5, 5);
                if (startplayer)
                {
                    g.DrawString("1) AI", drawFont, blackBrush, 5, 50);
                    g.FillEllipse(redBrush, 150, 50, 30, 30);
                    g.DrawString("2) YOU", drawFont, blackBrush, 5, 100);
                    g.FillEllipse(blueBrush, 150, 100, 30, 30);
                }
                if (playerTurn == 1 && !startplayer)
                {
                    g.DrawString("AI turn", drawFont, blackBrush, 5, 50);
                    g.FillEllipse(redBrush, 150, 50, 30, 30);
                }
                else if (playerTurn == 2 && !startplayer)
                {
                    g.DrawString("YOUR turn", drawFont, blackBrush, 5, 50);
                    g.FillEllipse(blueBrush, 150, 50, 30, 30);
                }
            }
            else
            {
                g.DrawString("press enter to start the game:", drawFont, blackBrush, 5, 5);
            }
        }
    }
}