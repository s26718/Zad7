using System.Data.Common;
using System.Data.SqlClient;
using Zad7.Models;

namespace Zad7.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private IConfiguration _configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM PRODUCT";
        var list = new List<Product>();

        using (var dr = await cmd.ExecuteReaderAsync())
        {
            while (await dr.ReadAsync())
            {
                list.Add(new Product
                {
                    IdProduct = Int32.Parse(dr["IdProduct"].ToString()),
                    Name = dr["Name"].ToString(),
                    Description = dr["Description"].ToString(),
                    Price = Double.Parse(dr["Price"].ToString())

                });
            }
        }

        return list;

    }

    public async Task<IEnumerable<Order>> GetOrdersAsync()
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM [ORDER]";
        var list = new List<Order>();


        using (var dr = await cmd.ExecuteReaderAsync())
        {
            while (await dr.ReadAsync())
            {
                list.Add(new Order
                {
                    IdOrder = Int32.Parse(dr["IdOrder"].ToString()),
                    IdProduct = Int32.Parse(dr["IdProduct"].ToString()),
                    Amount = Int32.Parse(dr["Amount"].ToString()),
                    CreatedAt = DateTime.Parse(dr["CreatedAt"].ToString()),
                    FulfilledAt = dr["FulfilledAt"].ToString() == ""
                        ? null
                        : DateTime.Parse(dr["FulfilledAt"].ToString())
                });
            }
        }


        return list;

    }

    public async Task<IEnumerable<Warehouse>> GetWarehousesAsync()
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM WAREHOUSE";
        var list = new List<Warehouse>();

        using (var dr = await cmd.ExecuteReaderAsync())
        {
            while (await dr.ReadAsync())
            {
                list.Add(new Warehouse
                {
                    IdWarehouse = Int32.Parse(dr["IdWarehouse"].ToString()),
                    Name = dr["Name"].ToString(),
                    Address = dr["Address"].ToString()
                });
            }
        }

        return list;
    }

    public async Task<Warehouse?> GetWarehouseAsync(int id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM WAREHOUSE WHERE IdWarehouse = @IdWarehouse";
        cmd.Parameters.AddWithValue("@IdWarehouse", id);


        using (var dr = await cmd.ExecuteReaderAsync())
        {
            while (await dr.ReadAsync())
            {
                return new Warehouse
                {
                    IdWarehouse = Int32.Parse(dr["IdWarehouse"].ToString()),
                    Name = dr["Name"].ToString(),
                    Address = dr["Address"].ToString()
                };
            }
        }

        return null;
    }

    public async Task<Product?> GetProductAsync(int id)
    {
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM PRODUCT WHERE IdProduct = @IdProduct";
        cmd.Parameters.AddWithValue("@IdProduct", id);


        using (var dr = await cmd.ExecuteReaderAsync())
        {
            while (await dr.ReadAsync())
            {
                return new Product
                {
                    IdProduct = Int32.Parse(dr["IdProduct"].ToString()),
                    Name = dr["Name"].ToString(),
                    Description = dr["Description"].ToString(),
                    Price = Double.Parse(dr["Price"].ToString())

                };
            }
        }

        return null;
    }

    public async Task<Order?> GetUnfulfilledOrderAsync(int idProduct, int amount)
    {
        {
            using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
            await con.OpenAsync();

            using var cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM [ORDER] WHERE IdProduct = @IdProduct  AND Amount = @Amount AND FulfilledAt IS NULL";
            cmd.Parameters.AddWithValue("@IdProduct", idProduct);
            cmd.Parameters.AddWithValue("@Amount", amount);


            using (var dr = await cmd.ExecuteReaderAsync())
            {
                while (await dr.ReadAsync())
                {
                    return new Order
                    {
                        IdOrder = Int32.Parse(dr["IdOrder"].ToString()),
                        IdProduct = Int32.Parse(dr["IdProduct"].ToString()),
                        Amount = Int32.Parse(dr["Amount"].ToString()),
                        CreatedAt = DateTime.Parse(dr["CreatedAt"].ToString()),
                        FulfilledAt = dr["FulfilledAt"].ToString() == ""
                            ? null
                            : DateTime.Parse(dr["FulfilledAt"].ToString())
                    };
                }
            }


            return null;
        }
    }

    public async Task<int> FulfillOrderAsync(int idWarehouse, int idProduct, int idOrder, int amount, double price)
    {
        int insertedRecordId = -1;
        String dateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        DateTime now = DateTime.Now;
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        using var cmd = new SqlCommand();
        cmd.Connection = con;

        DbTransaction tran = await con.BeginTransactionAsync();
        cmd.Transaction = (SqlTransaction)tran;


        cmd.CommandText = "UPDATE [ORDER] SET FulfilledAt = @FulfilledAt WHERE IdOrder = @IdOrder;" +
                          "INSERT INTO PRODUCT_WAREHOUSE(IdWarehouse,IdProduct,IdOrder,Amount,Price,CreatedAt) VALUES(@IdWarehouse,@IdProduct,@IdOrder,@Amount,@Price,@CreatedAt)" +
                          "SELECT SCOPE_IDENTITY()";


        cmd.Parameters.AddWithValue("@IdWarehouse", idWarehouse);
        cmd.Parameters.AddWithValue("@IdProduct", idProduct);
        cmd.Parameters.AddWithValue("@IdOrder", idOrder);
        cmd.Parameters.AddWithValue("@FulfilledAt", now.ToString(dateTimeFormat));
        cmd.Parameters.AddWithValue("@CreatedAt", now.ToString(dateTimeFormat));
        cmd.Parameters.AddWithValue("@Amount", amount);
        cmd.Parameters.AddWithValue("@Price", amount * price);

        try
        {
            insertedRecordId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }
        catch (SqlException ex)
        {

            Console.WriteLine("error in transaction");
            tran.Rollback();
        }

        tran.Commit();

        return insertedRecordId;
    }

    public async Task<bool> CheckIfProductWarehouseExists(int idOrder)
    {
        int count = 0;
        using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();

        using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM Product_Warehouse WHERE IdOrder = @IdOrder";
        cmd.Parameters.AddWithValue("@IdOrder", idOrder);


        using (var dr = await cmd.ExecuteReaderAsync())
        {
            
            while (await dr.ReadAsync())
            {
                count++;
            }
        }

        return count > 0;
    }
}