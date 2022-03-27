using System.Windows.Forms;
namespace Space_Invaders
{
    internal class SpaceShip : GameObject
    {
        int speed = 20;
        public SpaceShip(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.width = 60;
            this.height = 60;
            rect = new Rectangle(x, y, width, height);
        }
        public void Move(int screenWidth, string direction)
        {
            if (direction == "L")
            {
                if (x - speed < 0)
                {
                    x = 0;
                }
                else
                {
                    x -= speed;
                }
            }
            else
            {
                if (x + speed > screenWidth - width)
                {
                    x = screenWidth - width;
                }
                else
                {
                    x += speed;
                }
            }
            rect.X = x; 
        }

    }
}
