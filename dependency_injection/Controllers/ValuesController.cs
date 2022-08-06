using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using dependency_injection.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dependency_injection.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public ValuesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: api/values
        [HttpGet]
        public JsonResult Get()
        {
            DataTable data = new DataTable();
            string sql = _configuration.GetConnectionString("staff_conn");
            MySqlDataReader read;
            //List<staff> data_list = new List<staff>();
            using (MySqlConnection conn = new MySqlConnection(sql))
            {
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("dis_staff", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    read = command.ExecuteReader();
                    data.Load(read);
                    read.Close();
                    conn.Close();
                }
            }
            return new JsonResult(data);
        }
        [HttpPost]
        public string Post(staff st)
        {
            DataTable data = new DataTable();
            string sql = _configuration.GetConnectionString("staff_conn");
            MySqlDataReader read;
            using(MySqlConnection conn=new MySqlConnection(sql))
            {
                conn.Open();
                using(MySqlCommand command=new MySqlCommand("in_staff",conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@name", st.Name);
                    command.Parameters.AddWithValue("@id", st.Id);
                    command.Parameters.AddWithValue("@department", st.Department);
                    read = command.ExecuteReader();
                    data.Load(read);
                    read.Close();
                    conn.Close();

                }
            }
            return "insrted value";
        }
        [HttpPut]
        public string update(staff st)
        {
            DataTable data = new DataTable();
            MySqlDataReader read;
            string sql = _configuration.GetConnectionString("staff_conn");
            using(MySqlConnection conn=new MySqlConnection(sql))
            {
                conn.Open();
                using(MySqlCommand command=new MySqlCommand("up_staff",conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@pid", st.Id);
                    command.Parameters.AddWithValue("@name", st.Name);
                    command.Parameters.AddWithValue("@department", st.Department);
                    read = command.ExecuteReader();
                    data.Load(read);
                    read.Close();
                    conn.Close();
                }
            }
            return "updated successfully";
        }
        [HttpDelete]
        public string Delete(staff st)
        {
            DataTable data = new DataTable();
            MySqlDataReader read;
            string sql = _configuration.GetConnectionString("staff_conn");
            using (MySqlConnection conn = new MySqlConnection(sql))
            {
                conn.Open();
                using (MySqlCommand command = new MySqlCommand("del_staff",conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@pid", st.Id);
                    read = command.ExecuteReader();
                    read.Close();
                    conn.Close();
                }
            }
            return "Deleted successfully";
        }
    }
}

