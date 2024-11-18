namespace DAO
{
    using Entities;
    using Microsoft.Extensions.Logging;
    using MySqlConnector;

    public class GetUser
    {
        public readonly string _connectionString;
        public GetUser(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("SELECT * FROM Usuarios WHERE Deleted = 0", connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            User user = new User
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Nombre = reader["Nombre"].ToString(),
                                Edad = Convert.ToInt32(reader["Edad"])
                            };
                            users.Add(user);
                        }
                    }
                    connection.Close();
                    return users;
                }
            }
        }
        public User GetUserByID(int id)
        {
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand("SELECT ID,Nombre,Edad FROM usuarios WHERE ID = @ID AND DELETED = 0", connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        return new User
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Nombre = reader["Nombre"].ToString(),
                            Edad = Convert.ToInt32(reader["Edad"])
                        };
                    }
                }
            }
            return null;
        }
        public bool SoftDeleteUser(int userID)
        {
            var userFound = GetUserByID(userID);
            if (userFound == null)
            {
                Console.WriteLine("UserID was incorrect");
                return false;
            }

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("UPDATE usuarios SET Deleted = 1 WHERE ID = @userID;", connection))
                    {
                        command.Parameters.AddWithValue("@userID", userID);
                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }
        public User UpdateUser(int userID, string nombre, int edad)
        {
            var userFound = GetUserByID(userID);
            if (userFound == null)
            {
                Console.WriteLine("UserID es incorrecto");
                throw new Exception("El usuario no existe.");
            }

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand updateCommand = new MySqlCommand("UPDATE usuarios SET Nombre = @nombre, Edad = @edad WHERE ID = @id AND DELETED = 0", connection))
                {
                    updateCommand.Parameters.AddWithValue("@nombre", nombre);
                    updateCommand.Parameters.AddWithValue("@edad", edad);
                    updateCommand.Parameters.AddWithValue("@id", userID);

                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        Console.WriteLine("No se pudo actualizar el usuario. Verifique el ID.");
                        return null;
                    }
                }

                using (MySqlCommand selectCommand = new MySqlCommand("SELECT Nombre, Edad FROM usuarios WHERE ID = @id AND DELETED = 0", connection))
                {
                    selectCommand.Parameters.AddWithValue("@id", userID);

                    using (MySqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                Nombre = reader.GetString("Nombre"),
                                Edad = reader.GetInt32("Edad"),
                                ID = userID
                            };
                        }
                    }
                }
            }
            return null;
        }

    }
}
