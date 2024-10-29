
using Library_Management_System.MyConnection;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Library_Management_System.Operations
{
    public class Operation
    {
        public static SqlConnection sqlConnection()
        {
            SqlConnection con = new SqlConnection(Connection.ReadMyCon());
            return con;
        }
        public static SqlCommand sqlCommand(string Sp, SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand(Sp, con);
                cmd.CommandType = CommandType.StoredProcedure;
                return cmd;
        }

        public static int GetIdFromToken(string token)
        {
            var jwttokenhandler = new JwtSecurityTokenHandler();
            var jwttoken = jwttokenhandler.ReadJwtToken(token);
            var claims = jwttoken.Claims;

            var userid = claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);

            if (userid != null)
                return int.Parse(userid.Value);

            throw new Exception("User ID not found in token");
        }
        public static string GetData<T>(string Sp, T? model = default)
        {
            string JsonString = "";
            using (SqlConnection con = sqlConnection())
            {
                con.Open();
                using (SqlCommand cmd = sqlCommand(Sp, con))
                {
                    if(model != null)
                    {
                        var json = JsonConvert.SerializeObject(model);
                        cmd.Parameters.AddWithValue("@JsonString", json);
                    }
                    JsonString = (string)cmd.ExecuteScalar();
                }
            }
            return JsonString;
        }
        // INSERT AND UPDATE BOOK
        public static bool OpertionDataBook<T>(string Sp, T getModel)
        {
            using (SqlConnection con = sqlConnection())
            {
                con.Open();
                using (SqlCommand cmd = sqlCommand(Sp, con))
                {
                    string JsonString = JsonConvert.SerializeObject(getModel);
                    cmd.Parameters.AddWithValue("@JsonString", JsonString);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }
        // OPERATION TO INSERT INTO TABLE SAVES AND RESERVATION
        public static bool InsertData(string Sp, int? UserId = null, int? BookId = null)
        {
            using (SqlConnection con = sqlConnection())
            {
                con.Open();
                using (SqlCommand cmd = sqlCommand(Sp,con))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@BookId", BookId);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }
        public static bool Delete(string Sp, int UserId, int? BookId = null)
        {
            using (SqlConnection con = sqlConnection())
            {
                con.Open();
                using (SqlCommand cmd = sqlCommand(Sp, con))
                {
                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    if (BookId.HasValue)
                        cmd.Parameters.AddWithValue("@BookId", BookId);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }
    }
}
