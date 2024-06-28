using HR_sistem.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace HR_sistem.Repositories
{
    
        public class ReportsRepo : RepositoryBase
        {
            public ObservableCollection<PerformanceReview> GetReviews(int zaposleniId)
            {
                var reviews = new ObservableCollection<PerformanceReview>();

                using (var connection = GetConnection())
                using (var command = new SqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = @"
                    SELECT 
                        p.ID,
                        p.ReviewDate,
                        p.Score,
                        p.Comments
                    FROM PerformanceReview p
                    WHERE p.ZaposleniID = @ZaposleniID";
                    command.Parameters.Add("@ZaposleniID", SqlDbType.Int).Value = zaposleniId;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var review = new PerformanceReview
                            {
                                ID = reader.GetInt32(0),
                                ReviewDate = (DateTime)(reader.IsDBNull(1) ? (DateTime?)null : reader.GetDateTime(1)),
                                Score = reader.IsDBNull(2) ? 0 : reader.GetInt32(2),
                                Comments = reader.IsDBNull(3) ? null : reader.GetString(3)
                            };
                            reviews.Add(review);
                        }
                    }
                }

                return reviews;
            }

            public void AddReview(PerformanceReview review, int ZaposleniID)
            {
                try
                {
                    using (var connection = GetConnection())
                    using (var command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        command.CommandText = @"INSERT INTO PerformanceReview
                    (ZaposleniID, ReviewDate, Score, Comments) 
                    VALUES (@ZaposleniID, @ReviewDate, @Score, @Comments)";
                        command.Parameters.Add("@ZaposleniID", SqlDbType.Int).Value = ZaposleniID;
                        command.Parameters.Add("@ReviewDate", SqlDbType.DateTime).Value = review.ReviewDate;
                        command.Parameters.Add("@Score", SqlDbType.Int).Value = review.Score;
                        command.Parameters.Add("@Comments", SqlDbType.NVarChar).Value = review.Comments;
                        command.ExecuteNonQuery();

                    }

                }
                catch (SqlException ex)
                {
                    System.Windows.MessageBox.Show(ex.ToString());
                }

            }
        }
    }
 
    
