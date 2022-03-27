using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Invaders
{
    internal class Alien : GameObject
    {
        public int scoreCost;
        public int speed;
        public bool moveDown;
        public string dir;
        public Alien(int x, int y)
        {
            dir = "R";
            speed = 2;
            this.x = x;
            this.y = y;
            this.width = 30;
            height = 30;
            rect = new Rectangle(x, y, width, height);
        }

        public void Move(int screenWidth)
        {
            if (dir == "R")
            {
                if (x + speed < screenWidth - width)
                {
                    x += speed;
                }
                else
                {
                    moveDown = true;
                    x = screenWidth - width;
                }
            }
            else
            {
                if (x - speed > 0)
                {
                    x -= speed;
                }
                else
                {

                    moveDown = true;
                    x = 0;
                }
            }
            rect.X = x;
            rect.Y = y;
        }

    }
}
