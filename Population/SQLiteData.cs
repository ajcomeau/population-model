using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Population
{    
    class SQLiteData
    {
        private SQLiteConnection liteConn = new SQLiteConnection("data source = population.db");

        public int AddMember()
        {

            try
            {
                string sqlCode = "INSERT INTO members (CreateDate) VALUES (current_timestamp);SELECT last_insert_rowid();";
                SQLiteCommand sqlCmd = new SQLiteCommand(sqlCode, liteConn);
                int retValue = 0;

                liteConn.Open();

                retValue = int.Parse(sqlCmd.ExecuteScalar().ToString());

                liteConn.Close();

                return retValue;
            }
            catch(Exception ex)
            {
                return 0;
            }

        }

        public bool RemoveMember(int MemberID)
        {
            try
            {
                bool retValue;
                string sqlCode = "UPDATE members SET DestroyDate = current_timestamp WHERE memberID = " + MemberID.ToString() + ";";
                SQLiteCommand sqlCmd = new SQLiteCommand(sqlCode, liteConn);

                liteConn.Open();

                retValue = (sqlCmd.ExecuteNonQuery() > 0);

                liteConn.Close();

                return retValue;
            }
            catch (Exception ex)
            {
                return false;
            }

        }



    }
}
