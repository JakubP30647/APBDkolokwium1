using APBDkolokwium1.Exceptions;
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


    async Task IDeliveryServices.addNewDelivery(postDeliveryDTO newDeliveryDto)
    {
        String command = "";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync();


            cmd.CommandText = @"SELECT COUNT(*) FROM Delivery WHERE Delivery.delivery_id = @id";

            cmd.Parameters.AddWithValue("@id", newDeliveryDto.deliveryId);

            var a = (int)await cmd.ExecuteScalarAsync();

            if (a > 0)
            {
                throw new ConflictException(" Delivery o podanym ID już istnieje");
            }

            cmd.Parameters.Clear();

            cmd.CommandText = "SELECT COUNT(*) FROM Customer WHERE Customer.customer_id = @id";
            cmd.Parameters.AddWithValue("@id", newDeliveryDto.customerId);

            a = (int)await cmd.ExecuteScalarAsync();
            if (a < 1)
            {
                throw new NotFoundException(" Customer o podanym ID nie istnieje");
            }

            cmd.Parameters.Clear();
            cmd.CommandText =
                @"SELECT COUNT(*) FROM Driver WHERE driver_id  = (SELECT driver_id FROM Driver where Driver.licence_number = @licence_number)";
            cmd.Parameters.AddWithValue("@licence_number", newDeliveryDto.licenceNumber);
            
            
            if (!((int)await cmd.ExecuteScalarAsync() > 0))
            {
                throw new NotFoundException(" Driver o podanym ID nie istnieje");
            }

            cmd.Parameters.Clear();

            cmd.CommandText =
                @"INSERT INTO Delivery VALUES (@delivery_id, @customer_id, (SELECT driver_id FROM Driver where Driver.licence_number = @licence_number), GETDATE())";
            cmd.Parameters.AddWithValue("@delivery_id", newDeliveryDto.deliveryId);
            cmd.Parameters.AddWithValue("@customer_id", newDeliveryDto.customerId);
            cmd.Parameters.AddWithValue("@licence_number", newDeliveryDto.licenceNumber);
            
            

            
            await cmd.ExecuteNonQueryAsync();
            foreach (var s in newDeliveryDto.Products)
            {
               
                cmd.Parameters.Clear();
                cmd.CommandText = @"SELECT COUNT(*) FROM Product WHERE name = @name";
                cmd.Parameters.AddWithValue("@name", s.Name);
                
                if (!((int)await cmd.ExecuteScalarAsync() > 0))
                {
                    throw new NotFoundException("Product o takiej nazwie nie istnieje");
                }
            
           
                cmd.Parameters.Clear();
                cmd.CommandText = @"INSERT INTO Product_Delivery (delivery_id, product_id, amount)
                VALUES(@deliver_id,(SELECT product_id FROM Product where name = @name),@amount);";

                cmd.Parameters.AddWithValue("@deliver_id", newDeliveryDto.deliveryId);
                cmd.Parameters.AddWithValue("@name", s.Name);
                cmd.Parameters.AddWithValue("@amount", s.Amount);
            
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}