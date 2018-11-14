using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Population
{
    class MemberStats
    {
        //private SQLiteData dataAcc = new SQLiteData();
        private int health;
        private int xDir;
        private int yDir;
        private bool solid;
        private bool live;
        private int memid;

        public MemberStats(int Health, int XDirect, int YDirect)
        {
            this.HealthPoints = Health;
            this.XDirect = XDirect;
            this.YDirect = YDirect;
            //this.MemberID = dataAcc.AddMember();
        }

        public int MemberID
        {
            get { return memid; }
            set { memid = value; }
        }

        public bool Alive
        {
            get { return live; }
            set { live = value; }
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

                // If the health points are 0, make the 
                // object non-solid and no longer alive.
                if (health <= 0)
                {
                    this.Solid = false;
                    this.Alive = false;
                    //dataAcc.RemoveMember(this.memid);
                }
                else
                {
                    this.Alive = true;
                }
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
