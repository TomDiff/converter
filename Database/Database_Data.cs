using System;
using System.Collections.Generic;
using System.Text;
using Synios.Data.Access;
using System.Data;

namespace Database
{
    public class Database_Data
    {
        private Layer Layer = null;

        private static readonly Database_Data instance = new Database_Data();

        public static Database_Data Instance
        {
            get
            {
                return instance;
            }
        }

        Database_Data()
        {
        }

        public bool InitializedLayers(string Conn)
        {
            if (Layer != null)
                Disconect();

            Layer = new Layer(Conn);
            return Layer != null;
        }

        public void Disconect()
        {
            if (Layer == null)
                return;

            Layer.Close();
            Layer = null;
        }

        public void Execute(string query)
        {
            try
            {
                Layer.Execute(query, CommandType.Text);
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
            }
        }

        public DataTable ExecuteQuery(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                Layer.Execute(query, ref dt, CommandType.Text);
            }
            catch (Exception ex)
            {
                FileLogger.FileLogger.Instance.WriteExeption(ex);
            }
            return dt;
        }
    }
}
