using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Threading.Tasks;
using CRUDCLASSES.Model;

namespace CRUDCLASSES
{
    public class WhatsAppService : IWhatsAppServices
    {
        private readonly WhatsAppDbContext _dbContext;
        private bool _disposed;

        public WhatsAppService(WhatsAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<long> CreateUser(UserViewModel user)
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();

            string insertQuery =
                $"INSERT INTO USERS (UserName, Phone, Picture,About,Status_ID )"
                + $" VALUES (@UserName, @Phone, @Picture, @About,@Status_ID); SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";

            await using SqlCommand command = new SqlCommand(insertQuery, sqlConn);

            command.Parameters.AddRange(
                new SqlParameter[]
                {
                    new SqlParameter
                    {
                        ParameterName = "@UserName",
                        Value = user.UserName,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Size = 50
                    },
                    new SqlParameter
                    {
                        ParameterName = "@Phone",
                        Value = user.Phone,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Size = 50
                    },
                    new SqlParameter
                    {
                        ParameterName = "@Picture",
                        Value = user.Picture,
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Input,
                        Size = 50
                    },
                    new SqlParameter
                    {
                        ParameterName = "@About",
                        Value = user.About,
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Input,
                        Size = 50
                    },
                    new SqlParameter
                    {
                        ParameterName = "@Status_ID",
                        Value = user.Status_ID,
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Input,
                        Size = 50
                    }
                }
            );

            long userId = (long)await command.ExecuteScalarAsync();

            return userId;
        }

        public async Task<bool> UpdateUser(int userId, UserViewModel user)
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();

            string insertQuery =
                $"UPDATE Users SET UserName = @UserName, Phone = @Phone, Picture = @Picture,About = @About,Status_ID = @Status_ID WHERE UserID = @UserId ";

            await using SqlCommand command = new SqlCommand(insertQuery, sqlConn);

            command.Parameters.AddRange(
                new SqlParameter[]
                {
                    new SqlParameter
                    {
                        ParameterName = "@UserName",
                        Value = user.UserName,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Size = 50
                    },
                    new SqlParameter
                    {
                        ParameterName = "@UserId",
                        Value = userId,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Size = 50
                    },
                    new SqlParameter
                    {
                        ParameterName = "@Phone",
                        Value = user.Phone,
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Input,
                        Size = 50
                    },
                    new SqlParameter
                    {
                        ParameterName = "@Picture",
                        Value = user.Picture,
                        SqlDbType = SqlDbType.VarChar,
                        Direction = ParameterDirection.Input,
                        Size = 50
                    },
                    new SqlParameter
                    {
                        ParameterName = "@About",
                        Value = user.About,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Size = 50
                    },
                    new SqlParameter
                    {
                        ParameterName = "@Status_ID",
                        Value = user.Status_ID,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Size = 50
                    }
                }
            );

            var result = command.ExecuteNonQuery();

            return (result == 0) ? false : true;

            //throw new System.NotImplementedException();
        }

        public async Task<bool> DeleteUser(int UserId)
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();

            string deleteQuery = $"DELETE FROM Users WHERE UserId = @UserId ";
            await using SqlCommand command = new SqlCommand(deleteQuery, sqlConn);

            command.Parameters.AddRange(
                new SqlParameter[]
                {
                    new SqlParameter
                    {
                        ParameterName = "@UserId",
                        Value = UserId,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Size = 50
                    }
                }
            );

            var result = command.ExecuteNonQuery();

            return (result != 0);
        }

        public async Task<UserViewModel> GetUser(int id)
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();
            string getUserQuery =
                $"SELECT Users.UserName,Users.Phone,Users.Picture,Users.About,Users.Status_ID FROM Users WHERE UserId = @UserId ";
            await using SqlCommand command = new SqlCommand(getUserQuery, sqlConn);
            command.Parameters.AddRange(
                new SqlParameter[]
                {
                    new SqlParameter
                    {
                        ParameterName = "@UserId",
                        Value = id,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input,
                        Size = 50
                    }
                }
            );
            UserViewModel user = new UserViewModel();
            using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
            {
                while (dataReader.Read())
                {
                    user.UserName = dataReader["UserName"].ToString();
                    user.Phone = dataReader["Phone"].ToString();
                    user.Picture = dataReader["Picture"].ToString();
                    user.About = dataReader["About"].ToString();
                    user.Status_ID = dataReader["Status_ID"].ToString();
                }
            }

            return user;
        }

        public async Task<IEnumerable<UserViewModel>> GetUsers()
        {
            SqlConnection sqlConn = await _dbContext.OpenConnection();
            string getAllUsersQuery =
                $"SELECT Users.UserName,Users.Phone,Users.Picture,Users.About,Users.Status_ID FROM Users";
            await using SqlCommand command = new SqlCommand(getAllUsersQuery, sqlConn);
            List<UserViewModel> users = new List<UserViewModel>();
            using (SqlDataReader dataReader = await command.ExecuteReaderAsync())
            {
                while (dataReader.Read())
                {
                    users.Add(
                        new UserViewModel()
                        {
                            UserName = dataReader["UserName"].ToString(),
                            Phone = dataReader["Phone"].ToString(),
                            Picture = dataReader["Picture"].ToString(),
                            About = dataReader["About"].ToString(),
                            Status_ID = dataReader["Status_ID"].ToString()
                        }
                    );
                }
            }

            return users;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
