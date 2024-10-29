using Library_Management_System.MyConnection;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace Library_Management_System.DTOs
{
    public class UserRepo
    {
        static string connection;

        public UserRepo (string _connection)
        {
            connection = _connection;
        }
        /*
        هنا انا ببعت ال 
        USERNAME
        وبعدها ببعته للداتابيز بتاعتي تتاكد هل اليوزر ده موجود ولا لأ
        */
        public static string GetByUserName(string username)
        {
            using(SqlConnection con = new SqlConnection(Connection.ReadMyCon()))
            {
                string JsonString = "";
                using (SqlCommand cmd = new SqlCommand("GetByUsername", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@username", username);
                    con.Open();
                    JsonString = (string)cmd.ExecuteScalar();
                }
                return JsonString;
            }
        }
        /*
            HERE I WILL CHECK IF THE EMAIL USER WANT TO REGISTER IS EXISTS OR NOT.
            I WILL JUST USE IT IN OPERATION REGISTRATION.
        */
        public static bool GetByEmail(string Email)
        {
            using (SqlConnection con = new SqlConnection(Connection.ReadMyCon()))
            {
                using (SqlCommand cmd = new SqlCommand("GetByEmail", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", Email);
                    con.Open();
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }
        /*
            CHECK IF THE USER IS EXISTES OR NOT,
            IN LOGIN IF THE USER EXISTS THEN CONTINUE,
            IN REGISTRATION IF THE USER EXISTS THEN THROW EXIPTION "THE USER IS ALREADY EXISTS".
         */
        public static UserLoginRequest HasUser(string username)
        {
            var GetUserInfo = GetByUserName(username);
            if(GetUserInfo is null)
            {
                return new UserLoginRequest()
                {
                    Username = "Not found"
                };
            }
            return JsonConvert.DeserializeObject<UserLoginRequest>(GetUserInfo);
        }

        // ADD USER AND UPDATE USER PASSWORD.
        public static void UsersOperation<T>(T userRegistrition)
        {
            using (SqlConnection con = new SqlConnection(Connection.ReadMyCon()))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("OperationUsers", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string JsonString = JsonConvert.SerializeObject(userRegistrition);
                    cmd.Parameters.AddWithValue("@JsonString", JsonString);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
