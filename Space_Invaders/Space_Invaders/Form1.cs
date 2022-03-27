using System.Drawing;
using System.Collections.Generic;
namespace Space_Invaders
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;

        List<Bullet> bullets = new List<Bullet>();
        List<Bullet> enemyBullets = new List<Bullet>();
        int numberOfAlienRows = 5;
        List<List<Alien>> AlienRows = new List<List<Alien>>();
        SpaceShip spaceship;
        int LastShoot = 0;
        int LastEnemyShoot;
        int numberOfEnemies;
        int wave = 1;
        bool defeat = false;
        bool moveAliensDown = false;
        int current_score = 0;

        Image alienImage;
        Image spaceshipImage;
        public Form1()
        {
            InitializeComponent();
            resetAliens();
            mainGameTimer.Interval = 20;
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            spaceship = new SpaceShip(pictureBox1.Width/2, Convert.ToInt16(pictureBox1.Height*0.9));
            //some images
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo di = new DirectoryInfo(currentDirectory);
            string proj_root = di.Parent.Parent.Parent.FullName;
            spaceshipImage = Image.FromFile($"{proj_root}/Images/spaceship.png");
            alienImage = Image.FromFile($"{proj_root}/Images/alien.png");
        }
        void resetAliens()
        {
            AlienRows.Clear();
            for(int i = 0; i < numberOfAlienRows; i++)
            {
                AlienRows.Add(new List<Alien>());
                for(int j = 0; j < 9; j++)
                {
                    AlienRows[i].Add(new Alien(j * 60 + 45, 45 + i*60) { scoreCost = wave * 3 });
                }
            }
            LastEnemyShoot = new Random().Next(25, 35);
            
        }
        private void mainGameTimerTick(object sender, EventArgs e)
        {
            //game logic
            for(int i = 0; i < AlienRows.Count; i++)
            {
                if(AlienRows[i].Count == 0)
                {
                    AlienRows.Remove(AlienRows[i]);
                }
            }
            //moving enemies
            for(int i = 0; i < AlienRows.Count; i++)
            {
                for(int j = 0; j < AlienRows[i].Count; j++)
                {
                    AlienRows[i][j].Move(pictureBox1.Width);
                    if (AlienRows[i][j].moveDown)
                    {
                        moveAliensDown = true;
                        AlienRows[i][j].moveDown = false;
                    }
                }
            }

            if (moveAliensDown)
            {
                moveAliensDown = false;
                for(int i = 0; i < AlienRows.Count; i++)
                {
                    for(int j = 0; j < AlienRows[i].Count; j++)
                    {
                        AlienRows[i][j].y += 60;
                        AlienRows[i][j].rect.Y = AlienRows[i][j].y;

                        if (AlienRows[i][j].dir == "L")
                        {
                            AlienRows[i][j].dir = "R";
                        }
                        else
                        {
                            AlienRows[i][j].dir = "L";
                        }
                    }
                }
            }

            LastEnemyShoot--;

            if (LastShoot > 0)
            {
                LastShoot--;
            }

            numberOfEnemies = 0;
            for(int i = 0; i < AlienRows.Count; i++)
            {
                for(int j = 0; j < AlienRows[i].Count; j++)
                {
                    numberOfEnemies++;
                }
            }

            if(numberOfEnemies == 1)
            {
                for (int i = 0; i < AlienRows.Count; i++)
                {
                    for (int j = 0; j < AlienRows[i].Count; j++)
                    {
                        AlienRows[i].Remove(AlienRows[i][j]);
                    }
                }
            }
            if (numberOfEnemies == 0)
            {
                wave += 1;
                resetAliens();
            }
            //generating enemy bullets
            if (LastEnemyShoot == 0)
            {
                if(numberOfEnemies > 0)
                {
                    int randomRow = new Random().Next(0, AlienRows.Count - 1);
                    try
                    {
                        Alien shooter = AlienRows[randomRow][new Random().Next(0, AlienRows[randomRow].Count - 1)];
                        enemyBullets.Add(new Bullet(shooter.x + shooter.width / 2 + 5, shooter.y + 70, true));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    
                }
                LastEnemyShoot = new Random().Next(40, 50);
                
            }
            //moving player's bullets, and checking collision with enemies
            for(int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Move();

                for(int j = 0; j < AlienRows.Count; j++)
                {
                    for(int k = 0; k < AlienRows[j].Count; k++)
                    {
                        if (bullets[i].CheckIntersection(AlienRows[j][k]))
                        {
                            current_score += AlienRows[j][k].scoreCost;
                            AlienRows[j].Remove(AlienRows[j][k]);
                            bullets[i].isShooting = false;
                        }
                    }
                }
                if (!bullets[i].isShooting)
                {
                    bullets.Remove(bullets[i]);
                }
            }

            //moving enemy bullets and checking collision with player
            for (int i = 0; i < enemyBullets.Count; i++)
            {
                enemyBullets[i].Move();
                if (enemyBullets[i].CheckIntersection(spaceship))
                {
                    mainGameTimer.Stop();
                    defeat = true;
                    startGameBtn.Enabled = true;
                }
                if (!enemyBullets[i].isShooting)
                {
                    enemyBullets.Remove(enemyBullets[i]);
                }
                
            }

            // checking collision of player and alien
            
            for(int i = 0; i < AlienRows.Count; i++)
            {
                for(int j = 0; j < AlienRows[i].Count; j++)
                {
                    if (AlienRows[i][j].CheckIntersection(spaceship))
                    {
                        mainGameTimer.Stop();
                        defeat = true;
                        startGameBtn.Enabled = true;
                    }
                }
            }
             
               

            //drawing functions
            Graphics g = Graphics.FromImage(bitmap);
            g.Clear(Color.Black);

            for (int i = 0; i < AlienRows.Count; i++)
            {
                for (int j = 0; j < AlienRows[i].Count; j++)
                {
                    g.DrawImage(alienImage, AlienRows[i][j].rect);
                }
            }
           
            for (int i = 0; i < bullets.Count; i++)
            {
                if (bullets[i].isShooting)
                {
                    g.FillRectangle(new SolidBrush(Color.Red), bullets[i].rect);
                }
            }

            for (int i = 0; i < enemyBullets.Count; i++)
            {
                if (enemyBullets[i].isShooting)
                {
                    g.FillRectangle(new SolidBrush(Color.Red), enemyBullets[i].rect);
                }
            }

            g.DrawImage(spaceshipImage, spaceship.rect);
            if (!defeat)
            {
                g.DrawString($"SCORE:{current_score}", new Font("SegoeUi", 20), new SolidBrush(Color.Green), 0, 0);
            }
            else
            {
                g.DrawString($"DEFEAT, press restart button", new Font("SegoeUi", 20), new SolidBrush(Color.Green), 0, 0);
            }
            pictureBox1.Image = bitmap;
        }

        private void startGameBtn_Click(object sender, EventArgs e)
        {
            mainGameTimer.Enabled = true;
            startGameBtn.Text = "Restart";
            enemyBullets.Clear();
            bullets.Clear();
            startGameBtn.Enabled = false;
            current_score = 0;
            wave = 1;
            resetAliens();
            defeat = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                spaceship.Move(pictureBox1.Width, "L");
            }
            else if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                spaceship.Move(pictureBox1.Width, "R");
            }
            else if(e.KeyCode == Keys.Space)
            {
                if (LastShoot == 0)
                {
                    bullets.Add(new Bullet(spaceship.x + spaceship.width / 2 - 5, spaceship.y - 10, false));
                    LastShoot = 18;
                }
            }
            
        }
    }
}