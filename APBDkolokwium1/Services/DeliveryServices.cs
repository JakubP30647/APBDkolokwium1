using APBDkolokwium1.Models;
using Microsoft.Data.SqlClient;

namespace APBDkolokwium1.Services;

public class DeliveryServices : IDeliveryServices
{
    private readonly string _connectionString =
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kolokwium;Integrated Security=True;";

    async Task<DeliveryDTO> IDeliveryServices.getA(string visitId)
    {
        DeliveryDTO? result = null;

        string command = @"SELECT * FROM Visit 
            JOIN Client ON Visit.client_id = Client.client_id
            JOIN Mechanic ON Visit.mechanic_id = Mechanic.mechanic_id
            JOIN Visit_Service ON Visit.visit_id = Visit_Service.visit_id
            JOIN Service ON Visit_Service.service_id = Service.service_id
            WHERE Visit.visit_id = @id";


        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync();
            cmd.Parameters.AddWithValue("@id", int.Parse(visitId));

            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    
                    DateTime date = DateTime.Parse(reader["date"].ToString());



                    if (result == null)
                    {
                        
                        
                    }
                    else
                    {
                        
                        
                        
                        
                        
                    }

                }
            }
        }
        return result;
    }



    async Task IDeliveryServices.addNewA(postDeliveryDTO neewA)
    {
        
        
         String command = "";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync();

         

            // var a = (int)await cmd.ExecuteScalarAsync();
            //
            // if (a > 0)
            // {
            //     throw new ConflictException(" o podanym ID już istnieje");
            // }

       

            // a = (int)await cmd.ExecuteScalarAsync();
            // if (a < 1)
            // {
            //     throw new NotFoundException(" podanym ID nie istnieje");
            // }

            
            //
            // if (!((int)await cmd.ExecuteScalarAsync() > 0))
            // {
            //     throw new NotFoundException(" o podanym ID nie istnieje");
            // }

            cmd.Parameters.Clear();

           
            //
            // await cmd.ExecuteNonQueryAsync();
            // foreach (var a in neewA.aaa)
            // {
            //    
            //
            //     // if (!((int)await cmd.ExecuteScalarAsync() > 0))
            //     // {
            //     //     throw new NotFoundException("o takiej nazwie nie istnieje");
            //     // }
            //
            //
            //     cmd.Parameters.Clear();
            //     
            //     // insert
            //
            //     await cmd.ExecuteNonQueryAsync();
            // }
        }
        
        
    }
    
}