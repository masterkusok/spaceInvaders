using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Invaders
{
    internal class Bullet : GameObject
    {
        public int speed = 20;
        public bool isShooting = false;
        public bool isEnemy;
        public Bullet(int x, int y, bool isEnemy)
        {
            this.isEnemy = isEnemy;
            isShooting = true;
            this.x = x;
            this.y = y;
            this.width = 10;
            this.height = 20;
            this.rect = new Rectangle(this.x, this.y, width, height);
        }
        public void Move()
        {
            if (isShooting)
            {
                if (!isEnemy)
                {
                    if (y + height >= 0)
                    {
                        y -= speed;
                    }
                    else
                    {
                        isShooting = false;
                        y = -100;
                    }
                    rect.Y = y;
                }
                else
                {
                    if (y -height <= 748)
                    {
                        y += speed;
                    }
                    else
                    {
                        isShooting = false;
                    }
                    rect.Y = y;
                }
            }
            
        }
    }
}
