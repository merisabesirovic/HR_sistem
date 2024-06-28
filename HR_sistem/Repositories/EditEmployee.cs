using HR_sistem.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HR_sistem.Repositories
{
    public class EditEmployee : RepositoryBase
    {
        public void Edit(Zaposleni updatedUser)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = @"
                        UPDATE Zaposleni 
                        SET Name = @Name, 
                            Surname = @Surname, 
                            Contact = @Contact, 
                            Address = @Address, 
                            Position = @Position, 
                            WorkingHours = @WorkingHours, 
                            PayingHour = @PayingHour, 
                            DateOfStart = @DateOfStart, 
                            OdeljenjeID = @OdeljenjeID
                        WHERE ID = @ID";
                    command.Parameters.Add("@ID", SqlDbType.Int).Value = updatedUser.ID;
                    command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = updatedUser.Name;
                    command.Parameters.Add("@Surname", SqlDbType.NVarChar).Value = updatedUser.Surname;
                    command.Parameters.Add("@Contact", SqlDbType.NVarChar).Value = updatedUser.Contact;
                    command.Parameters.Add("@Address", SqlDbType.NVarChar).Value = updatedUser.Address;
                    command.Parameters.Add("@Position", SqlDbType.NVarChar).Value = updatedUser.Position;
                    command.Parameters.Add("@WorkingHours", SqlDbType.Int).Value = updatedUser.WorkingHours;
                    command.Parameters.Add("@PayingHour", SqlDbType.Float).Value = updatedUser.PayingHour;
                    command.Parameters.Add("@DateOfStart", SqlDbType.DateTime).Value = updatedUser.DateOfStart;
                    command.Parameters.Add("@OdeljenjeID", SqlDbType.Int).Value = updatedUser.Odeljenje.ID;
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error updating employee: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void Delete(int employeeId)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;

                    
                    using (var transaction = connection.BeginTransaction())
                    {
                        command.Transaction = transaction;

                        
                        command.CommandText = "DELETE FROM PerformanceReview WHERE ZaposleniID = @ZaposleniID";
                        command.Parameters.Clear();
                        command.Parameters.Add("@ZaposleniID", SqlDbType.Int).Value = employeeId;
                        command.ExecuteNonQuery();

                        command.CommandText = "DELETE FROM Benefits WHERE ZaposleniID = @ZaposleniID";
                        command.Parameters.Clear();
                        command.Parameters.Add("@ZaposleniID", SqlDbType.Int).Value = employeeId;
                        command.ExecuteNonQuery();

                        
                        command.CommandText = "DELETE FROM Zaposleni WHERE ID = @ID";
                        command.Parameters.Clear();
                        command.Parameters.Add("@ID", SqlDbType.Int).Value = employeeId;
                        command.ExecuteNonQuery();

                       
                        transaction.Commit();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error deleting employee: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
