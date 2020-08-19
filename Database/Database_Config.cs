using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using Synios.Data.Access;

namespace Database
{
    public class Database_Config
    {
        private Layer Layer = null;

        private static readonly Database_Config instance = new Database_Config();

        public static Database_Config Instance
        {
            get
            {
                return instance;
            }
        }

        Database_Config()
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
