using APBDkolokwium1.Models;
using Microsoft.Data.SqlClient;

namespace APBDkolokwium1.Services;

public class DeliveryServices : IDeliveryServices
{
    private readonly string _connectionString =
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=kolokwium;Integrated Security=True;";

    async Task<DeliveryDTO> IDeliveryServices.getDelivery(string visitId)
    {
        DeliveryDTO? result = null;

        string command =
            @"SELECT *, Driver.first_name as driver_first_name, Driver.last_name as driver_last_name FROM Delivery
        JOIN Customer ON Delivery.customer_id = Customer.customer_id
        JOIN Driver ON Delivery.driver_id = Driver.driver_id
        JOIN Product_Delivery ON Delivery.delivery_id = Product_Delivery.delivery_id
        JOIN Product ON Product_Delivery.product_id = Product.product_id
        WHERE Delivery.delivery_id = @id";


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
                    Customer customer = new Customer()
                    {
                        FirstName = reader["first_name"].ToString(),
                        LastName = reader["last_name"].ToString(),
                        DateOfBirth = DateTime.Parse(reader["date_of_birth"].ToString()),
                    };

                    Driver driver = new Driver()
                    {
                        FirstName = reader["driver_first_name"].ToString(),
                        LastName = reader["driver_last_name"].ToString(),
                        LicenceNumber = reader["licence_number"].ToString(),
                    };


                    Product product = new Product()
                    {
                        Name = reader["name"].ToString(),
                        Price = Convert.ToDouble(reader["price"]),
                        Amount = (int)reader["amount"],
                    };

                    if (result == null)
                    {
                        result = new DeliveryDTO()
                        {
                            Date = date,
                            Customer = customer,
                            Driver = driver,
                            Products = new List<Product>(),
                        };


                        result.Products.Add(product);
                    }
                    else
                    {
                        result.Products.Add(product);
                    }
                    
                }
            }
        }

        return result;
    }


    async Task IDeliveryServices.addNewDelivery(postDeliveryDTO neewA)
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