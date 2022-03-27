using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Invaders
{
    internal class GameObject
    {
        public int x;
        public int y;
        public int width;
        public int height;
        public Rectangle rect;

        public bool CheckIntersection(GameObject obj)
        {
            if((this.x >= obj.x && this.x <= obj.x + obj.width || this.x + width >= obj.x && this.x+width <= obj.x+obj.width)&&
            (this.y >= obj.y && this.y <= obj.y + obj.height || this.y + height >= obj.y && this.y + height <= obj.y + obj.height))
            {
                return true;
            }    
            return false;
        }
    }
}
