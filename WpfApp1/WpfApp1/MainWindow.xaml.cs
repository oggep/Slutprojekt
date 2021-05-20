using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;
using System.IO;
namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        Rect playerHitBox;
        Rect groundHitBox;
        Rect obstacleHitBox;

        bool jumping;

        int force = 20;
        int speed = 5;

        Random rnd = new Random();

        bool gameover = true;

        double spriteIndex = 0;

        ImageBrush playerSprite = new ImageBrush();
        ImageBrush backgroundSprite = new ImageBrush();
        ImageBrush obstacleSprite = new ImageBrush();

        int[] obstaclePosition = { 320, 310, 300, 305, 315 };

        int score = 0;
        public MainWindow()
        {
            InitializeComponent();

            MyCanvas.Focus();
            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            var doesExist = File.Exists("image/background.gif");
            backgroundSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/background.gif"));

            Background.Fill = backgroundSprite;
            Background2.Fill = backgroundSprite;

            //gameTimer.Start();

        }
        
        private void GameEngine(object sender, EventArgs e){
            Canvas.SetLeft(Background, Canvas.GetLeft(Background) - 3);
            Canvas.SetLeft(Background, Canvas.GetLeft(Background2) - 3);
            
            if (Canvas.GetLeft(Background) < -1262)
            {
                Canvas.SetLeft(Background, Canvas.GetLeft(Background2) + Background2.Width);
            }
            if (Canvas.GetLeft(Background2) < -1262)
            {
                Canvas.SetLeft(Background2, Canvas.GetLeft(Background2) + Background2.Width);
            }

            Canvas.SetTop(player, Canvas.GetTop(player) + speed);
            Canvas.SetLeft(Obstacle, Canvas.GetLeft(Obstacle) - 12);

            scoretext.Content = "Score: " + score;
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width - 15, player.Height);
            obstacleHitBox = new Rect(Canvas.GetLeft(Obstacle), Canvas.GetTop(Obstacle), Obstacle.Width - 15, Obstacle.Height - 10);
            groundHitBox = new Rect(Canvas.GetLeft(Ground), Canvas.GetTop(Ground), Ground.Width - 15, Ground.Height - 10);
            
            if (playerHitBox.IntersectsWith(groundHitBox))
            {
                speed = 0;

                Canvas.SetTop(player, Canvas.GetTop(Ground) - player.Height);

                jumping = false;

                spriteIndex += .5;

                if(spriteIndex > 8)
                {
                    spriteIndex = 1;
                }
                RunSprite(spriteIndex);
            }
            if(jumping == true)
            {
                speed = -9;

                force -= 1;
            }
            else 
            {
                speed = 12;
            }
            if(force < 0)
            {
                jumping = false;
            }
            if (Canvas.GetLeft(Obstacle) < -50)
            {
                Canvas.SetLeft(Obstacle, 950);

                Canvas.SetTop(Obstacle, obstaclePosition[rnd.Next(0, obstaclePosition.Length)]);

                score += 1;

            }
             if(playerHitBox.IntersectsWith(obstacleHitBox)){
                gameover = true;

                gameTimer.Stop();
            }
             if(gameover = true)
            {
                Obstacle.Stroke = Brushes.Black;
                Obstacle
            }
        }

        private void IntersectsWith(Rect obstacleHitBox)
        {
            throw new NotImplementedException();
        }

        private void RunSprite(double i)
        {
            switch (i)
            {
                case 1:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_01.gif"));
                    break;
                case 2:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_02.gif"));
                    break;
                case 3:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_03.gif"));
                    break;
                case 4:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_04.gif"));
                    break;
                case 5:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_05.gif"));
                    break;
                case 6:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_06.gif"));
                    break;
                case 7:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_07.gif"));
                    break;
                case 8:
                    playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_08.gif"));
                    break;
            }

            player.Fill = playerSprite;

        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && gameover == true)
            {
                StartGame();
            }
        }
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space && jumping == false && Canvas.GetTop(player) > 260)
            {
                jumping = true;
                force = 15;
                speed = -12;

                playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_02.gif"));
            }
        }
        private void StartGame()
        {
            Canvas.SetLeft(Background, 0);
            Canvas.SetLeft(Background, 1262);

            Canvas.SetLeft(player, 110);
            Canvas.SetTop(Obstacle, 140);

            Canvas.SetLeft(Obstacle, 950);
            Canvas.SetTop(Obstacle, 310);

            RunSprite(1);

            obstacleSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/obstacle.png"));
            Obstacle.Fill = obstacleSprite;

            jumping = false;
            gameover = false;
            score = 0;

            scoretext.Content = "Score:" + score;

            gameTimer.Start();
        }
    }
}
