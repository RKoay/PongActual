/*
 * Description:     A basic PONG simulator
 * Author:          RIE K
 * Date:            
 */

#region libraries

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;

#endregion

namespace Pong
{
    public partial class Form1 : Form
    {
        #region global values

        //graphics objects for drawing
        SolidBrush drawBrush = new SolidBrush(Color.LightPink);
        SolidBrush drawScores = new SolidBrush(Color.White);
        SolidBrush drawBlocks1 = new SolidBrush(Color.Black);
        SolidBrush drawBlocks2 = new SolidBrush(Color.LightBlue);
        Font drawFont = new Font("Courier New", 10);

        // Sounds for game
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.score);
        SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);
        //Adding a new sound
        SoundPlayer paddleSound = new SoundPlayer(Properties.Resources.Metal_Clang_SoundBible_com_1363525732);

        //determines whether a key is being pressed or not
        Boolean aKeyDown, zKeyDown, jKeyDown, mKeyDown;

        // check to see if a new game can be started
        Boolean newGameOk = true;

        //ball directions, speed, and rectangle
        Boolean ballMoveRight = true;
        Boolean ballMoveDown = true;
        const int BALL_SPEED = 4;
        Rectangle ball;

        //paddle speeds and rectangles
        const int PADDLE_SPEED = 4;
        Rectangle p1, p2;

        //player and game scores
        int player1Score = 0;
        int player2Score = 0;
        int gameWinScore = 5;  // number of points needed to win game

        

        

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.A:
                    aKeyDown = true;
                    break;
                case Keys.Z:
                    zKeyDown = true;
                    break;
                case Keys.J:
                    jKeyDown = true;
                    break;
                case Keys.M:
                    mKeyDown = true;
                    break;
                case Keys.Y:
                case Keys.Space:
                    if (newGameOk)
                    {
                        SetParameters();
                    }
                    break;
                case Keys.N:
                    if (newGameOk)
                    {
                        Close();
                    }
                    break;
            }
        }

        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.A:  //temp comment
                    aKeyDown = false;
                    break;
                case Keys.Z:
                    zKeyDown = false;
                    break;
                case Keys.J:
                    jKeyDown = false;
                    break;
                case Keys.M:
                    mKeyDown = false;
                    break;
            }
        }



        /// <summary>
        /// sets the ball and paddle positions for game start
        /// </summary>
        private void SetParameters()
        {
            if (newGameOk)
            {
                player1Score = player2Score = 0;
                newGameOk = false;
                startLabel.Visible = false;
                gameUpdateLoop.Start();
            }

            //set starting position for paddles on new game and point scored 
            const int PADDLE_EDGE = 20;  // buffer distance between screen edge and paddle      

            p1.Width = p2.Width = 20;    //height for both paddles set the same
            p1.Height = p2.Height = 60;  //width for both paddles set the same

            //p1 starting position
            p1.X = PADDLE_EDGE;
            p1.Y = this.Height / 2 - p1.Height / 2;

            //p2 starting position
            p2.X = this.Width - PADDLE_EDGE - p2.Width;
            p2.Y = this.Height / 2 - p2.Height / 2;

            // Set Width and Height of ball
            ball.Height = ball.Width = 20;
            // Set starting X position for ball to middle of screen, (use this.Width and ball.Width)
            ball.X = this.Width / 2 - ball.Width / 2;
            // Set starting Y position for ball to middle of screen, (use this.Height and ball.Height)
            ball.Y = this.Height / 2 - ball.Height / 2;
            

        }
        

        /// <summary>
        /// This method is the game engine loop that updates the position of all elements
        /// and checks for collisions.
        /// </summary>
        private void gameUpdateLoop_Tick(object sender, EventArgs e)
        {
            
            
            #region update ball position

            // Create code to move ball either left or right based on ballMoveRight and using BALL_SPEED
            // Create code move ball either down or up based on ballMoveDown and using BALL_SPEED

            if (ballMoveRight == true)
            {
                //ball moves right
                Graphics g = this.CreateGraphics();
                g.FillEllipse(drawBrush, ball.X = ball.X + BALL_SPEED, ball.Y, ball.Width, ball.Height);
            }
            else
            {
                //ball moves left
                Graphics g = this.CreateGraphics();
                g.FillEllipse(drawBrush, ball.X = ball.X - BALL_SPEED, ball.Y, ball.Width, ball.Height);
            }
            if (ballMoveDown == true)
            {
                //ball moves down 
                Graphics g = this.CreateGraphics();
                g.FillEllipse(drawBrush, ball.X, ball.Y = ball.Y + BALL_SPEED, ball.Width, ball.Height);
            }
            else
            {
                //ball moves up
                Graphics g = this.CreateGraphics();
                g.FillEllipse(drawBrush, ball.X, ball.Y = ball.Y - BALL_SPEED, ball.Width, ball.Height);
            }



            #endregion

            #region update paddle positions

            if (aKeyDown == true && p1.Y > 0)
            {
                // Move player 1 paddle up using p1.Y and PADDLE_SPEED
                Graphics g = this.CreateGraphics();
                g.FillRectangle(drawBrush, p1.X, p1.Y = p1.Y - PADDLE_SPEED, p1.Width, p1.Height);
            }

            if (zKeyDown == true && p1.Y > 0)
            {
                // Move player 1 paddle down using p1.Y and PADDLE_SPEED
                Graphics g = this.CreateGraphics();
                g.FillRectangle(drawBrush, p1.X, p1.Y = p1.Y + PADDLE_SPEED, p1.Width, p1.Height);

            }

            if (jKeyDown == true && p2.Y > 0)
            {
                // Move player 2 paddle up using p2.Y and PADDLE_SPEED
                Graphics g = this.CreateGraphics();
                g.FillRectangle(drawBrush, p2.X, p2.Y = p2.Y - PADDLE_SPEED, p2.Width, p2.Height);
            }

            if (mKeyDown == true && p2.Y > 0)
            {
                // Move player 2 paddle down using p2.Y and PADDLE_SPEED
                Graphics g = this.CreateGraphics();
                g.FillRectangle(drawBrush, p2.X, p2.Y = p2.Y + PADDLE_SPEED, p2.Width, p2.Height);
            }

            // Both paddles cannot be too above and too below 
            if (p1.Y < 20)
            {
                p1.Y = 20;
            }
            if (p1.Y + p1.Height > this.Height - 20)
            {
                p1.Y = this.Height - 20 - p1.Height;
            }
            if (p2.Y < 20)
            {
                p2.Y = 20;
            }
            if (p2.Y + p2.Height > this.Height - 20)
            {
                p2.Y = this.Height - 20 - p2.Height;
            }


            #endregion

            #region ball collision with top and bottom lines

            if (ball.Y < 0) // if ball hits top line
            {
                // TODO use ballMoveDown boolean to change direction
                ballMoveDown = true;
                // TODO play a collision sound
                collisionSound.Play();
                this.BackColor = Color.Blue;
            }
            // TODO In an else if statement use ball.Y, this.Height, and ball.Width to check for collision with bottom line
            // If true use ballMoveDown down boolean to change direction
            if (ball.Y > this.Height - ball.Height)
            {
                ballMoveDown = false;
                collisionSound.Play();
                this.BackColor = Color.Green;
            }

            #endregion

            #region ball collision with paddles
            // Create if statment that checks p1 collides with ball and if it does
            // --- play a "paddle hit" sound and

            // --- use ballMoveRight boolean to change direction
            if (ball.IntersectsWith(p1))
            {
                ballMoveRight = true;
                paddleSound.Play();
                this.BackColor = Color.DarkRed;
            }

            // Create if statment that checks p2 collides with ball and if it does
            // --- play a "paddle hit" sound and
            // --- use ballMoveRight boolean to change direction
            if (ball.IntersectsWith(p2))
            {
                ballMoveRight = false;
                paddleSound.Play();
                this.BackColor = Color.Black;
            }

          

            /*  ENRICHMENT
             *  Instead of using two if statments as noted above see if you can create one
             *  if statement with multiple conditions to play a sound and change direction
             */

            #endregion

            #region ball collision with side walls (point scored)

            if (ball.X < 0)  // ball hits left wall logic
            {
                // --- play score sound
                scoreSound.Play();
                // --- update player 2 score
                player2Score = player2Score + 1;
                

                // TODO use if statement to check to see if player 2 has won the game. If true run 
                // GameOver method. Else change direction of ball and call SetParameters method.
                if (player2Score == gameWinScore)
                {
                    GameOver("Player #2 wins");
                    this.BackColor = Color.Orange;
                }
                else
                {
                    ballMoveRight = true;
                    //SetParameters();
                    this.BackColor = Color.Orange;
                }


            }

            // TODO same as above but this time check for collision with the right wall 
            if (ball.X > this.Width - ball.Width)
            {
                scoreSound.Play();
                player1Score = player1Score + 1;

                if (player1Score == gameWinScore)
                {
                    GameOver("Player #1 wins");
                    this.BackColor = Color.Brown;
                }
                else
                {
                    ballMoveRight = false;
                    //SetParameters();
                    this.BackColor = Color.Brown;
                }
            }


            #endregion

            //refresh the screen, which causes the Form1_Paint method to run
            this.Refresh();
        }
        
        /// <summary>
        /// Displays a message for the winner when the game is over and allows the user to either select
        /// to play again or end the program
        /// </summary>
        /// <param name="winner">The player name to be shown as the winner</param>
        private void GameOver(string winner)
        {
            newGameOk = true;

            // Create game over logic
            // --- stop the gameUpdateLoop
            gameUpdateLoop.Stop();
            // --- show a message on the startLabel to indicate a winner, (need to Refresh).

            startLabel.Visible = true;
            startLabel.Text = winner + "";
            Refresh();

            // --- pause for two seconds 
            Thread.Sleep(2000);
            // --- use the startLabel to ask the user if they want to play again
            startLabel.Text = "Do you want to play again ?\nPress Space To Start A New Game";

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            string player1 = player1Score.ToString();
            string player2 = player2Score.ToString();
            // TODO draw paddles using FillRectangle
            e.Graphics.FillRectangle(drawBrush, p1);
            e.Graphics.FillRectangle(drawBrush, p2);
            // TODO draw ball using FillRectangle
            e.Graphics.FillEllipse(drawBrush, ball);
            // TODO draw scores to the screen using DrawString
            e.Graphics.DrawString("Player 1 Score\n" + "      " + player1, drawFont, drawScores, this.Width/16, this.Height / 8);
            e.Graphics.DrawString("Player 2 Score\n" + "      " + player2, drawFont, drawScores, this.Width - this.Width/3, this.Height / 8);

            

        }

    }
}
