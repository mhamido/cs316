using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ai
{
    public enum Phase
    {
        SettingBoardSize,
        SettingFirstMoveAndStreak,
        GameInProgress,
    }

    public partial class Controller : Form
    {
        private Bitmap off;
        private Board board = null;
        private Agent agent = new Agent();
        private Human player = new Human();
        private int columns = 7, rows = 6, playerTurn = 1, move = 0;
        private bool StartGame = true, startplayer = true;
        private List<Bitmap> number = new List<Bitmap>();

        public Controller()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Load += Form1_Load;
            this.Paint += Form1_Paint1;
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            this.MouseClick += Controller_MouseClick;
            Init();
        }

        private void Init()
        {
            agent = new Agent();
            player = new Human();
            StartGame = true;
            startplayer = true;
        }

        private void CheckForWin()
        {
            Cell cell = board.HasWon();
            switch (cell)
            {
                case Cell.Empty:
                    DrawDubb(CreateGraphics());
                    return;
                case Cell.AgentPiece:
                    MessageBox.Show("The agent has won!");
                    break;
                case Cell.HumanPiece:
                    MessageBox.Show("The human has won!");
                    break;
            }

            Init();
            DrawDubb(CreateGraphics());
        }

        private void Controller_MouseClick(object sender, MouseEventArgs e)
        {
            int x = (Width - (columns * 75)) / 2, y = 10, x1 = x;
            if (!StartGame && !startplayer && playerTurn == 2)
            {
                for (int i = 1; i < columns + 1; i++)
                {
                    if (e.X > x1 && e.X < x + 75 * i && e.Y > y && e.Y < y + rows * 75)
                    {
                        board.MakeMove(player, i - 1);
                        CheckForWin();

                        // TODO: Maybe decouple the rendering of the board before and after the agent's turn?
                        int col = agent.Deliberate(board);

                        board.MakeMove(agent, col);
                        CheckForWin();
                    }
                    x1 = x + 75 * i;
                }
            }
            DrawDubb(CreateGraphics());
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (StartGame && columns < 10)
                    {
                        columns++;
                        board = null;
                    }
                    if (!StartGame && startplayer)
                    {
                        if (move >= 0 && move < 5)
                        {
                            move++;
                        }
                    }
                    break;
                case Keys.Down:
                    if (StartGame && columns > 5)
                    {
                        columns--;
                        board = null;
                    }
                    if (!StartGame && startplayer)
                    {
                        if (move > 0 && move <= 6)
                        {
                            move--;
                        }
                    }
                    break;
                case Keys.Left:
                    if (StartGame && rows < 10)
                    {
                        rows++;
                        board = null;
                    }
                    break;
                case Keys.Right:
                    if (StartGame && rows > 5)
                    {
                        rows--;
                        board = null;
                    }
                    break;
                case Keys.Enter:
                    StartGame = false;
                    break;
                case Keys.D1:
                    if (!StartGame && startplayer)
                    {
                        board = new Board(rows, columns, move + 4);
                        startplayer = false;
                        playerTurn = 1;
                        int col = agent.Deliberate(board);
                        board.MakeMove(agent, col);
                        DrawDubb(CreateGraphics());
                        playerTurn = 2;
                    }
                    break;
                case Keys.D2:
                    if (!StartGame && startplayer)
                    {
                        board = new Board(rows, columns, move + 4);
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
            for (int k = 4; k < 10; k++)
            {
                Bitmap img = new Bitmap(k + ".jpg");
                number.Add(img);
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
            SolidBrush redBrush = new SolidBrush(Color.Red);
            SolidBrush blueBrush = new SolidBrush(Color.Blue);
            SolidBrush yellowBrush = new SolidBrush(Color.Yellow);
            SolidBrush grayBrush = new SolidBrush(Color.Gray);
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            Font drawFont = new Font("Arial", 16);
            Pen blackPen = new Pen(Color.Black, 5);
            int x = (Width - (columns * 75)) / 2, y = 10;
            g.DrawString("win by connect:", drawFont, blackBrush, 100, 300);
            g.DrawImage(number[move], 250, 298);
            for (int i = 0; i < rows; i++)
            {
                for (int k = 0; k < columns; k++)
                {
                    g.FillRectangle(yellowBrush, x, y, 75, 75);
                    if (board != null) switch (board.Cells[i, k])
                        {
                            case Cell.Empty:
                                g.FillEllipse(grayBrush, x, y, 70, 70);
                                break;
                            case Cell.AgentPiece:
                                g.FillEllipse(redBrush, x, y, 70, 70);
                                break;
                            case Cell.HumanPiece:
                                g.FillEllipse(blueBrush, x, y, 70, 70);
                                break;
                        }
                    else
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
                g.DrawString("Press enter to start the game:", drawFont, blackBrush, 5, 5);
            }
        }
    }
}