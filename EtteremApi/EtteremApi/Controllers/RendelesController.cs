using EtteremApi.Models;
using EtteremApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace EtteremApi.Controllers
{
    [Route("rendeles")]
    [ApiController]
    public class RendelesController : ControllerBase
    {
        public string ConnectionString = "server=localhost;database=etterem;uid=root;password=";

        [HttpGet("all")]
        public object GetAllRendeles()
        {
            List<Rendeles> rendelesek = new List<Rendeles>();

            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"SELECT * FROM `rendeles`";

            var cmd = new MySqlCommand(sql, connector);

            var datareader = cmd.ExecuteReader();

            while (datareader.Read())
            {
                var rendeles = new Rendeles
                {
                    Id = datareader.GetInt32(0),
                    Dish = datareader.GetString(1),
                    Description = datareader.GetString(2),
                    OrderTime = datareader.GetDateTime(3),
                    UpdateTime = datareader.GetDateTime(4),
                    VendegId = datareader.GetInt32(5)
                };

                rendelesek.Add(rendeles);
            }

            connector.Close();

            return new { message = "Sikeres lekérdezés.", result = rendelesek };
        }

        [HttpGet("byId")]
        public object GetRendelesById([FromQuery] int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"SELECT * FROM `rendeles` WHERE `id` = @id;";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            datareader.Read();

            var rendeles = new Rendeles
            {
                Id = datareader.GetInt32(0),
                Dish = datareader.GetString(1),
                Description = datareader.GetString(2),
                OrderTime = datareader.GetDateTime(3),
                UpdateTime = datareader.GetDateTime(4),
                VendegId = datareader.GetInt32(5)
            };

            connector.Close();
            return new { message = "Sikeres találat.", result = rendeles };
        }

        [HttpPost("add")]
        public object AddNewRendeles(AddNewRendelesDto addNewRendelesDto)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"INSERT INTO `rendeles`(`dish`, `description`, `orderTime`, `updateTime`, `vendegId`) VALUES (@dish,@description,@orderTime,@updateTime,@vendegId)";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@dish", addNewRendelesDto.Dish);
            cmd.Parameters.AddWithValue("@description", addNewRendelesDto.Description);
            cmd.Parameters.AddWithValue("@orderTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@vendegId", addNewRendelesDto.VendegId);

            cmd.ExecuteNonQuery();

            connector.Close();

            return new { message = "Sikeres hozzáadás.", result = addNewRendelesDto };
        }

        [HttpPut("update")]
        public object UpdateRendeles([FromQuery] int id, [FromBody] UpdateRendelesDto updateRendelesDto)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"UPDATE `rendeles` SET `dish`=@dish,`description`=@description,`updateTime`=@updateTime,`vendegId`=@vendegId 
               WHERE `id`= @id;";

            var cmd = new MySqlCommand(sql, connector);

            cmd.Parameters.AddWithValue("@dish", updateRendelesDto.Dish);
            cmd.Parameters.AddWithValue("@description", updateRendelesDto.Description);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@vendegId", updateRendelesDto.VendegId);
            cmd.Parameters.AddWithValue("@id", id);

            object result = cmd.ExecuteNonQuery() > 0 ? new { message = "Sikeres frissítés." } : new { message = "Nincs ilyen rendelés." };

            connector.Close();

            return result;
        }

        [HttpDelete("deleteById")]
        public object DeleteRendeles([FromBody] int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"DELETE FROM `rendeles` WHERE `id` = @id";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            object result = cmd.ExecuteNonQuery() > 0 ? new { message = "Sikeres törlés." } : new { message = "Nincs ilyen rendelés." };

            connector.Close();

            return result;
        }

        [HttpGet("vendegNameEmail")]
        public object GetVendegNameAndEmail([FromQuery] int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"SELECT `name`, `email` FROM `vendeg` WHERE `id` = @id;";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            datareader.Read();

            var result = new
            {
                Name = datareader.GetString(0),
                Email = datareader.GetString(1)
            };

            connector.Close();
            return new { message = "Sikeres lekérdezés.", result = result };
        }

        [HttpGet("vendegWithRendelesek")]
        public object GetVendegWithRendelesek([FromQuery] int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"SELECT `name` FROM `vendeg` WHERE `id` = @id;";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            datareader.Read();

            string name = datareader.GetString(0);

            datareader.Close();

            string sql2 = @"SELECT `dish`, `description` FROM `rendeles` WHERE `vendegId` = @id;";

            var cmd2 = new MySqlCommand(sql2, connector);
            cmd2.Parameters.AddWithValue("@id", id);

            var datareader2 = cmd2.ExecuteReader();

            List<object> rendelesek = new List<object>();

            while (datareader2.Read())
            {
                rendelesek.Add(new
                {
                    Dish = datareader2.GetString(0),
                    Description = datareader2.GetString(1)
                });
            }

            connector.Close();

            return new { message = "Sikeres lekérdezés.", result = new { Name = name, Rendelesek = rendelesek } };
        }

        [HttpGet("totalCount")]
        public object GetTotalRendelesCount()
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"SELECT COUNT(*) FROM `rendeles`";

            var cmd = new MySqlCommand(sql, connector);

            var datareader = cmd.ExecuteReader();

            datareader.Read();

            int count = datareader.GetInt32(0);

            connector.Close();

            return new { message = "Sikeres lekérdezés.", result = count };
        }

        [HttpGet("countByVendeg")]
        public object GetRendelesCountByVendeg([FromQuery] int id)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = @"SELECT COUNT(*) FROM `rendeles` WHERE `vendegId` = @id;";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);

            var datareader = cmd.ExecuteReader();

            datareader.Read();

            int count = datareader.GetInt32(0);

            connector.Close();

            return new { message = "Sikeres lekérdezés.", result = count };
        }
    }
}
