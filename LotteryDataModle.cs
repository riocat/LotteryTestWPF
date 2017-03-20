using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.Common;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections.ObjectModel;

namespace WpfApplication1
{
    public class LotteryDataModle
    {
        private string conString = "Database='lottery';Data Source='localhost';User Id='root';Password='abcd1234';charset='utf8'";

        public string ConString
        {
            get { return conString; }
            set { conString = value; }
        }

        private static LotteryDataModle instance;

        private static Object lockObj = new Object();

        public static LotteryDataModle getInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    instance = new LotteryDataModle();
                    return instance;
                }
            }
            else
            {
                return instance;
            }
        }

        private LotteryDataModle()
        {
        }

        public void Insert(ObservableCollection<Lottery> lotteryList)
        {
            String sql = "INSERT INTO `lottery`.`lottery` (`no`, `ball1`, `ball2`, `ball3`, `ball4`, `ball5`, `ball6`, `ball7`, `result_string`) VALUES (@no, @ball1, @ball2, @ball3, @ball4, @ball5, @ball6, @ball7, @result_string)";
            MySqlParameter[] paramters = { new MySqlParameter("@no", MySqlDbType.Int32), 
                                         new MySqlParameter("@ball1",MySqlDbType.Int32),
                                         new MySqlParameter("@ball2",MySqlDbType.Int32),
                                         new MySqlParameter("@ball3",MySqlDbType.Int32),
                                         new MySqlParameter("@ball4",MySqlDbType.Int32),
                                         new MySqlParameter("@ball5",MySqlDbType.Int32),
                                         new MySqlParameter("@ball6",MySqlDbType.Int32),
                                         new MySqlParameter("@ball7",MySqlDbType.Int32),
                                         new MySqlParameter("@result_string",MySqlDbType.String,255)};

            using (MySqlConnection conneciton = new MySqlConnection(conString))
            {
                conneciton.Open();
                MySqlTransaction mst = conneciton.BeginTransaction();
                foreach (Lottery lottery in lotteryList)
                {

                    paramters[0].Value = lottery.No;
                    paramters[1].Value = lottery.Redball1;
                    paramters[2].Value = lottery.Redball2;
                    paramters[3].Value = lottery.Redball3;
                    paramters[4].Value = lottery.Redball4;
                    paramters[5].Value = lottery.Redball5;
                    paramters[6].Value = lottery.Blueball1;
                    paramters[7].Value = lottery.Blueball2;
                    paramters[8].Value = lottery.ResultString;

                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = sql;
                        command.Connection = conneciton;
                        command.Transaction = mst;
                        foreach (MySqlParameter pam in paramters)
                        {
                            command.Parameters.Add(pam);
                        }
                        command.ExecuteNonQuery();
                    }
                }
                mst.Commit();
                conneciton.Close();
            }

        }

        internal int getDataCount()
        {
            var sql = "SELECT COUNT(*) FROM lottery";
            using (MySqlConnection conneciton = new MySqlConnection(conString))
            {
                conneciton.Open();
                using (MySqlCommand command = new MySqlCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    command.Connection = conneciton;
                    Object num = command.ExecuteScalar();
                    return Convert.ToInt32(num);
                }
            }
        }

        internal Lottery getlastRecord()
        {
            Lottery result = null;
            var sql = "SELECT * FROM lottery where id = ( SELECT MAX(id) FROM lottery)";
            using (MySqlConnection conneciton = new MySqlConnection(conString))
            {
                DataSet ds = new DataSet();
                conneciton.Open();
                using (MySqlDataAdapter adpater = new MySqlDataAdapter(sql, conneciton))
                {
                    adpater.Fill(ds);


                    foreach (DataTable dt in ds.Tables)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            result = new Lottery();
                            result.No = Convert.ToInt32(dr[1]);
                            return result;
                        }
                    }
                }
            }
            return result;
        }

        internal ObservableCollection<Lottery> getLast100()
        {
            String sql = "SELECT * FROM lottery ORDER BY id DESC LIMIT 0,100 ";
            ObservableCollection<Lottery> list = new ObservableCollection<Lottery>();
            using (MySqlConnection connection = new MySqlConnection(ConString))
            {
                DataSet ds = new DataSet();
                using (MySqlDataAdapter msda = new MySqlDataAdapter(sql, connection))
                {
                    msda.Fill(ds);
                    foreach (DataTable dt in ds.Tables)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            Lottery result = new Lottery();
                            result.Id = Convert.ToInt32(dr[0]);
                            result.No = Convert.ToInt32(dr[1]);
                            result.Redball1 = Convert.ToInt32(dr[2]);
                            result.Redball2 = Convert.ToInt32(dr[3]);
                            result.Redball3 = Convert.ToInt32(dr[4]);
                            result.Redball4 = Convert.ToInt32(dr[5]);
                            result.Redball5 = Convert.ToInt32(dr[6]);
                            result.Blueball1 = Convert.ToInt32(dr[7]);
                            result.Blueball2 = Convert.ToInt32(dr[8]);
                            result.ResultString = Convert.ToString(dr[9]);
                            list.Add(result);
                        }
                    }
                }
            }
            return list;
        }
    }
}
