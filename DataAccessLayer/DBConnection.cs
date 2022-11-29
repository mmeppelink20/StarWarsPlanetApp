﻿using DataAccessLayerInterfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    internal class DBConnection : IDBConnection
    {
        public SqlConnection GetConnection()
        {
            SqlConnection conn = null;

            string connectionString = @"Data Source=localhost;Initial Catalog=sw_planets_db;Integrated Security=True";
            conn = new SqlConnection(connectionString);

            return conn;
        }
    }
}
