using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Population
{
    class MemberStats
    {
        private int health;
        private int xDir;
        private int yDir;
        private bool solid;

        public MemberStats(int Health, int XDirect, int YDirect)
        {
            this.health = Health;
            this.xDir = XDirect;
            this.yDir = YDirect;
        }

        public int HealthPoints
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
            }
        }

        public int XDirect
        {
            get
            {
                return xDir;
            }
            set
            {
                xDir = value;
            }
        }

        public int YDirect
        {
            get
            {
                return yDir;
            }
            set
            {
                yDir = value;
            }
        }

        public bool Solid
        {
            get
            {
                return solid;
            }
            set
            {
                solid = value;
            }
        }

    }
}
