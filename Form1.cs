using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>(); //I'm not motivated enough to comment why I made a list like this.  
        private Circle food = new Circle(); //making new private object for food

        public Form1() //this code goes to the Form1 designer 
        {
            InitializeComponent();

         
            new Settings(); //makes settings from settings class default to the game

            //Set game speed and start timer
            SnakegameTimer.Interval = 1000 / Settings.Speed;
            SnakegameTimer.Tick += UpdateScreen;
            SnakegameTimer.Start();

            
            StartGame(); //Starts a new game
        }

        private void StartGame()
        {
            lblGameOver.Visible = false;

            
            new Settings();//sets the settings to default in the game

            
            Snake.Clear(); //Creates a new player object
            Circle head = new Circle { X = 10, Y = 5 }; //Creates a new player object
            Snake.Add(head); //Creates a new player object


            lblScore.Text = Settings.Score.ToString(); //prints additive score in a message in game
            GenerateFood();

        }
        private void GenerateFood() //Places a random food on any x or y coordinate/pos
        {
            int maxXPos = pbCanvas.Size.Width / Settings.Width; //refer to comment above
            int maxYPos = pbCanvas.Size.Height / Settings.Height; //refer to comment above

            Random random = new Random();
            food = new Circle { X = random.Next(0, maxXPos), Y = random.Next(0, maxYPos) }; //refer to comment above
        }


        private void UpdateScreen(object sender, EventArgs e) //constantly updates the canvas with all objects inside it. 
        {
            if (Settings.GameOver) //Checks for Game Over
            {
                
                if (Input.KeyPressed(Keys.Enter)) //Checks if Enter is pressed
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPressed(Keys.Right) && Settings.Snakedirection != Direction.Left) //makes sure the snake goes right
                    Settings.Snakedirection = Direction.Right;

                else if (Input.KeyPressed(Keys.Left) && Settings.Snakedirection != Direction.Right) //makes sure the snake goes left
                    Settings.Snakedirection = Direction.Left;

                else if (Input.KeyPressed(Keys.Up) && Settings.Snakedirection != Direction.Down) //makes sure the snake goes up 
                    Settings.Snakedirection = Direction.Up;

                else if (Input.KeyPressed(Keys.Down) && Settings.Snakedirection != Direction.Up) //makes sure the snake goes down
                    Settings.Snakedirection = Direction.Down;

                MovePlayer(); //I will get to this further below. It does exactly how it is name
            }

            pbCanvas.Invalidate(); //allows stuff to be redrawn

        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e) 
        {
            Graphics canvas = e.Graphics; //Sets snake color

            if (!Settings.GameOver)
            {
                for (int i = 0; i < Snake.Count; i++) //for loop we learned in Dylan Johnson's CSC 250 class 
                {
                    Brush snakeColor;
                    if (i == 0)
                        snakeColor = Brushes.Black; //draws snake's head
                    else
                        snakeColor = Brushes.Green; //makes snake's body green

                    canvas.FillEllipse(snakeColor, //Draws snake
                        new Rectangle(Snake[i].X * Settings.Width,
                                      Snake[i].Y * Settings.Height,
                                      Settings.Width, Settings.Height));


                    canvas.FillEllipse(Brushes.Red, //Draws Food
                        new Rectangle(food.X * Settings.Width,
                             food.Y * Settings.Height, Settings.Width, Settings.Height));

                }
            }
            else //this prints results once your snake runs into a wall or itself
            {
                string gameOver = "Game over \n Your final score is: " + Settings.Score + " \n Press Enter to try again";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }


        private void MovePlayer()
        {
            for (int i = Snake.Count - 1; i >= 0; i--) //a for loop we learned in CSC 250
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
                    for (int j = 1; j < Snake.Count; j++) //more CSC 250 I learned with Dylan Johnson
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
            Settings.GameOver = true; //the snake isn't immortal now is it?
        }

        private void lblGameOver_Click(object sender, EventArgs e) // DO NOT REMOVE THIS
        {

        }

        private void Form1_Load(object sender, EventArgs e) //DO NOT REMOVE THIS
        {

        }
    }
}