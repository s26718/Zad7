using Zad7.Models;

namespace Zad7.Repositories;

public interface IWarehouseRepository
{
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<IEnumerable<Warehouse>> GetWarehousesAsync();
    Task<IEnumerable<Order>> GetOrdersAsync();
    Task<Warehouse?> GetWarehouseAsync(int id);
    Task<Product?> GetProductAsync(int id);
    Task<Order?> GetUnfulfilledOrderAsync(int idProduct, int amount);
    Task<bool> CheckIfProductWarehouseExists(int id);
    Task<int> FulfillOrderAsync(int idWarehouse, int idProduct, int idOrder, int amount, double price);
    Task<int> FulfillOrderWithProcedureAsync(int idWarehouse, int idProduct, int amount, DateTime requestDateTime);
    
    
    




}