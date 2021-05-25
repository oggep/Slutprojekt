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
        //Skapar en ny instans av DispatcherTimer som heter timer 
        DispatcherTimer gameTimer = new DispatcherTimer();

        //Hitboxar för objekten
        Rect playerHitBox;
        Rect groundHitBox;
        Rect obstacleHitBox;

        //Boolean för att känna av om gubben hoppar
        bool jumping;
        
        //variabler för kraft och hastighet
        int force = 20;
        int speed = 5;

        //Skapar instans av Random vid namn rnd
        Random rnd = new Random();

        //Boolean för att känna av om spelet är över eller inte
        bool gameover = true;

        //Animationer för sprites
        double spriteIndex = 0;
        //Skapar instanser av imagebrush för mina sprites/objekt
        ImageBrush playerSprite = new ImageBrush();
        ImageBrush backgroundSprite = new ImageBrush();
        ImageBrush obstacleSprite = new ImageBrush();

        //Höjd positionering för mitt hinder
        int[] obstaclePosition = { 320, 310, 300, 305, 315 };

        int score = 0;
        public MainWindow()
        {
            InitializeComponent();
            //
            MyCanvas.Focus();
            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            //Denna ger backgrounden sin bild och placering
            var doesExist = File.Exists("/images/background.gif");
            backgroundSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/background.gif"));

            Background.Fill = backgroundSprite;
            Background2.Fill = backgroundSprite;

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
             if(gameover)
            {
                Obstacle.Stroke = Brushes.Black;
                Obstacle.StrokeThickness = 1;

                player.Stroke = Brushes.Red;
                player.StrokeThickness = 1;

                scoretext.Content = "Score: " + score + " Press enter to play again!";
            }
            else
            {
                player.StrokeThickness = 0;
                Obstacle.StrokeThickness = 0;
            }
        }

        private void IntersectsWith(Rect obstacleHitBox)
        {
            throw new NotImplementedException();
        }
        //Denna metod sköter animationerna för min playersprite
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
        //Denna metod startar spelet när man trycker på enter-knappen. 
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            //if-satsen känner av om man trycker på enter och att spelet är över.
            if (e.Key == Key.Enter && gameover == true)
            {
                StartGame();
            }
        }
        //Denna metód gör att man kan hoppa i mitt spel när man trycker på space knappen.
        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            //if-satsen känner av om man trycker på space och att jumping är falskt och man har positionen vid marken.
            if (e.Key == Key.Space && jumping == false && Canvas.GetTop(player) > 260)
            {
                //dessa rader kod gör att jumping är sant, kraften ökar och hastigheten minskar.
                jumping = true;
                force = 15;
                speed = -12;

                //här byter player till hopp animationen.
                playerSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/newRunner_02.gif"));
            }
        }
        //Denna metod sköter starten oh återställningen av spelet. 
        private void StartGame()
        {
            //Återställer placering för bakgrund, player och hinder
            Canvas.SetLeft(Background, 0);
            Canvas.SetLeft(Background, 1262);

            Canvas.SetLeft(player, 110);
            Canvas.SetTop(player, 140);

            Canvas.SetLeft(Obstacle, 950);
            Canvas.SetTop(Obstacle, 310);

            //sätter bilden för hindret
            RunSprite(1);
            obstacleSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/obstacle.png"));
            Obstacle.Fill = obstacleSprite;

            //lägger bakgrunden för spelet.
            backgroundSprite.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/background.gif"));

            Background.Fill = backgroundSprite;
            Background2.Fill = backgroundSprite;

            //återställer värdena för jumping, gameover och score
            jumping = false;
            gameover = false;
            score = 0;

            //denna ändrar poängen i spelet
            scoretext.Content = "Score:" + score;

            //Denna startar om speltimern 
            gameTimer.Start();
        }
    }
}
