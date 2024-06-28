using HR_sistem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_sistem.Repositories
{
   public class BenefitsRepo : RepositoryBase
    {
        public ObservableCollection<Benefits> GetBenefits(int zaposleniID)
        {
            var benefits = new ObservableCollection<Benefits>();

            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
            SELECT b.ID,
                   b.BenefitName,
                   b.GrantedDate,
                   b.EndedDate
            FROM Benefits b
            WHERE b.ZaposleniID = @ZaposleniID";
                command.Parameters.Add("@ZaposleniID", SqlDbType.Int).Value = zaposleniID;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var benefit = new Benefits
                        {
                            ID = reader.GetInt32(0),
                            BenefitName = reader.GetString(1),
                            GrantedDate = reader.IsDBNull(2) ? (DateTime?)null : reader.GetDateTime(2),
                            EndedDate = reader.IsDBNull(3) ? (DateTime?)null : reader.GetDateTime(3)
                        };
                        benefits.Add(benefit);
                    }
                }
            }
            return benefits;
        }


        public void AddBenefit(Benefits benefit, int zaposleniID)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = @"
                    INSERT INTO Benefits (BenefitName, GrantedDate, EndedDate, ZaposleniID)
                    VALUES (@BenefitName, @GrantedDate, @EndedDate, @ZaposleniID)";
                command.Parameters.Add("@BenefitName", SqlDbType.NVarChar).Value = benefit.BenefitName;
                command.Parameters.Add("@GrantedDate", SqlDbType.DateTime).Value = benefit.GrantedDate;
                command.Parameters.Add("@EndedDate", SqlDbType.DateTime).Value = benefit.EndedDate;
                command.Parameters.Add("@ZaposleniID", SqlDbType.Int).Value = zaposleniID;

                command.ExecuteNonQuery();
            }
        }
    }
}
