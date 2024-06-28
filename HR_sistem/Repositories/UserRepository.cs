using HR_sistem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
//implementirano da vrati trenutnog korisnika tj HR koji koristi app
//i na osnovu trenutnog usera da vrati odeljenje kojim rukovodi
namespace HR_sistem.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from RadnikHR where UserName =@username and Password=@password";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;

                validUser = command.ExecuteScalar() == null ? false : true;
            }
            return validUser;

        }

        public ObservableCollection<Zaposleni> GetEmployeesByOdeljenje(int odeljenjeId)
        {
            var employees = new ObservableCollection<Zaposleni>();
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
            SELECT 
            *
            FROM Zaposleni z
            INNER JOIN Odeljenje o  ON z.OdeljenjeID = o.ID
            WHERE z.OdeljenjeID = @OdeljenjeID";
                command.Parameters.Add("@OdeljenjeID", System.Data.SqlDbType.Int).Value = odeljenjeId;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var employee = new Zaposleni
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Surname = reader.GetString(2),
                            Contact = reader.GetString(3),
                            Address = reader.GetString(4),
                            Position = reader.GetString(5),
                            WorkingHours = reader.GetInt32(6),
                            PayingHour = (float)reader.GetDouble(7),
                            DateOfStart = reader.GetDateTime(8),
                            Odeljenje = new Odeljenje { ID = reader.GetInt32(9), Name = reader.GetString(11) }
                        };
                        employees.Add(employee);
                    }
                }
            }
            return employees;
        }

        public RadnikHR GetbyUserName(string username)
        {
            RadnikHR user = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
            SELECT 
                z.ID,
                z.Name,
                z.Surname,
                z.Contact,
                z.Address,
                z.Position,
                z.WorkingHours,
                z.PayingHour,
                z.DateOfStart,
                z.OdeljenjeID,
                rh.UserName,
                rh.Password,
                o.Name AS OdeljenjeName
            FROM RadnikHR rh
            INNER JOIN Zaposleni z ON rh.ID = z.ID
            INNER JOIN Odeljenje o ON z.OdeljenjeID = o.ID
            WHERE rh.UserName = @username";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new RadnikHR()
                        {
                            ID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Surname = reader.GetString(2),
                            Contact = reader.GetString(3),
                            Address = reader.GetString(4),
                            Position = reader.GetString(5),
                            WorkingHours = reader.GetInt32(6),
                            PayingHour = (float)reader.GetDouble(7),
                            DateOfStart = reader.GetDateTime(8),
                            Odeljenje = new Odeljenje()
                            {
                                ID = reader.GetInt32(9),
                                Name = reader.GetString(12)
                            },
                            UserName = reader.GetString(10),
                            Password = reader.GetString(11)
                        };
                    }
                }
            }
            return user;
        }
    }
}
