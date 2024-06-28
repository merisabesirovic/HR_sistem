using HR_sistem.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HR_sistem.ViewModels;
using System.Data;
using System.Windows;

namespace HR_sistem.Repositories
{
    public class AddEmployee : RepositoryBase
    {
        public void Add(Zaposleni newUser)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = @"
                        INSERT INTO Zaposleni (ID, Name, Surname, Contact, Address, Position, WorkingHours, PayingHour, DateOfStart, OdeljenjeID) 
                        VALUES (@ID, @Name, @Surname, @Contact, @Address, @Position, @WorkingHours, @PayingHour, @DateOfStart, @OdeljenjeID)";
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = newUser.ID;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = newUser.Name;
                    command.Parameters.Add("@Surname", SqlDbType.NVarChar).Value = newUser.Surname;
                    command.Parameters.Add("@Contact", SqlDbType.NVarChar).Value = newUser.Contact;
                    command.Parameters.Add("@Address", SqlDbType.NVarChar).Value = newUser.Address;
                    command.Parameters.Add("@Position", SqlDbType.NVarChar).Value = newUser.Position;
                    command.Parameters.Add("@WorkingHours", SqlDbType.Int).Value = newUser.WorkingHours;
                    command.Parameters.Add("@PayingHour", SqlDbType.Float).Value = newUser.PayingHour;
                    command.Parameters.Add("@DateOfStart", SqlDbType.DateTime).Value = newUser.DateOfStart;
                    command.Parameters.Add("@OdeljenjeID", SqlDbType.Int).Value = newUser.Odeljenje.ID;
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex) when (ex.Number == 2627)
            {
                MessageBox.Show("An employee with this Birth certificate ID already exists. Please use a unique ID.", "Primary Key Violation", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
