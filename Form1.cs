using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();

            //Set settings to default
            new Settings();

            //Set game speed and start timer
            SnakegameTimer.Interval = 1000 / Settings.Speed;
            SnakegameTimer.Tick += UpdateScreen;
            SnakegameTimer.Start();

            //Start New game
            StartGame();
        }

        private void StartGame()
        {
            lblGameOver.Visible = false;

            //Set settings to default
            new Settings();

            //Create new player object
            Snake.Clear();
            Circle head = new Circle { X = 10, Y = 5 };
            Snake.Add(head);


            lblScore.Text = Settings.Score.ToString();
            GenerateFood();

        }

        //Place random food object
        private void GenerateFood()
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width;
            int maxYPos = pbCanvas.Size.Height / Settings.Height;

            Random random = new Random();
            food = new Circle { X = random.Next(0, maxXPos), Y = random.Next(0, maxYPos) };
        }


        private void UpdateScreen(object sender, EventArgs e)
        {
            //Check for Game Over
            if (Settings.GameOver)
            {
                //Check if Enter is pressed
                if (Input.KeyPressed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.Snakedirection != Direction.Left)
                    Settings.Snakedirection = Direction.Right;

                else if (Input.KeyPressed(Keys.Left) && Settings.Snakedirection != Direction.Right)
                    Settings.Snakedirection = Direction.Left;

                else if (Input.KeyPressed(Keys.Up) && Settings.Snakedirection != Direction.Down)
                    Settings.Snakedirection = Direction.Up;

                else if (Input.KeyPressed(Keys.Down) && Settings.Snakedirection != Direction.Up)
                    Settings.Snakedirection = Direction.Down;

                MovePlayer();
            }

            pbCanvas.Invalidate();

        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;

            if (!Settings.GameOver)
            {
                //Sets snake color

                //Draw snake
                for (int i = 0; i < Snake.Count; i++) //for loop we learned in Dylan Johnson's CSC 250 class with *actually programming demonstrations*
                {
                    Brush snakeColour;
                    if (i == 0)
                        snakeColour = Brushes.Black; //draws snake's head
                    else
                        snakeColour = Brushes.Green; //makes snake's body green

                    //Draw snake
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width, Settings.Height));


                    //Draw Food
                    canvas.FillEllipse(Brushes.Red,
                        new Rectangle(food.X * Settings.Width,
                             food.Y * Settings.Height, Settings.Width, Settings.Height));

                }
            }
            else
            {
                string gameOver = "Game over \nYour final score is: " + Settings.Score + "\nPress Enter to try again";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }


        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--) //a for loop we learned in CSC 250, WITH ACTUAL PROGRAMMING DEMONSTRATIONS FROM Dylan Johnson. Give him a thank you for teaching Jeffrey For Loops with i++
            {
                
                if (i == 0)
                {
                    switch (Settings.Snakedirection)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }

                    int maxXPos = pbCanvas.Size.Width / Settings.Width; //Get maximum X and Y Positions
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;//refer to comment above

                
                    if (Snake[i].X < 0 || Snake[i].Y < 0 //Detects collisions with borders
                        || Snake[i].X >= maxXPos || Snake[i].Y >= maxYPos) //Detects collisions with borders
                    {
                        Die();
                    }


                    //Detect collission with body
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].X == Snake[j].X &&
                           Snake[i].Y == Snake[j].Y)
                        {
                            Die();
                        }
                    }

                    //Detect collision with food in game
                    if (Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                        Eat();
                    }

                }
                else
                {
                    //Move body
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) //IF YOU REMOVE THIS THE SNAKE WILL NOT LOAD
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) //IF YOU REMOVE THIS THE SNAKE WILL NOT LOAD
        {
            Input.ChangeState(e.KeyCode, false);
        }

        private void Eat()
        {
         
            Circle circle = new Circle //Adds circles to snake body
            {
                X = Snake[Snake.Count - 1].X,
                Y = Snake[Snake.Count - 1].Y
            };
            Snake.Add(circle);

            //Update Score
            Settings.Score += Settings.Points; //this updates to lblScore Label
            lblScore.Text = Settings.Score.ToString();

            GenerateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void lblGameOver_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}