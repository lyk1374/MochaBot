using MochaBot.Common;
using MySqlConnector;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace MochaBot.Models
{
    public class MochaBotModel
    {
        public string writer { get; set; }
        public string timeStamp { get; set; }

        public bool ChkIsNew(string writer, string timeStamp)
        {
            bool isNew = false;
            string sSql = string.Empty;
            DataTable dt = new DataTable();

            using (var connection = new MySqlConnection(Const.DBConnection))
            {
                connection.Open();

                sSql = "SELECT * FROM timestamp WHERE WRITER = '" + writer + "'";

                MySqlCommand command = new MySqlCommand(sSql, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                

                adapter.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    sSql = "INSERT INTO timestamp(WRITER, TIMESTAMP) VALUES('" + writer + "', '" + timeStamp + "')";

                    command = new MySqlCommand(sSql, connection);

                    command.ExecuteNonQuery();

                    isNew = true;
                }
            }

            

            return isNew;
        }

        public int ChkTimeStamp(string writer, string timeStamp)
        {
            int timeCal = 0;
            string sSql = string.Empty;
            DataTable dt = new DataTable();

            using (var connection = new MySqlConnection(Const.DBConnection))
            {
                connection.Open();

                sSql = "SELECT TIMESTAMP FROM timestamp WHERE WRITER = '"+writer+"'";

                MySqlCommand command = new MySqlCommand(sSql, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);


                adapter.Fill(dt);

                string savedTimeStamp = dt.Rows[0][0].ToString();
                TimeSpan ts = Convert.ToDateTime(timeStamp) - Convert.ToDateTime(savedTimeStamp);
                timeCal = Convert.ToInt32(ts.TotalMinutes);
                if (timeCal > 5)
                {
                    sSql = "UPDATE timestamp SET TIMESTAMP = '" + timeStamp + "' WHERE WRITER = '"+writer+"'";
                    
                    command = new MySqlCommand(sSql, connection);

                    command.ExecuteNonQuery();

                }



            }
                return timeCal;
        }

    }
}
