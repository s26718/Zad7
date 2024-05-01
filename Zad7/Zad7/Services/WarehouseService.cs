using Zad7.Exceptions;
using Zad7.Models;
using Zad7.Repositories;

namespace Zad7.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<int> FulfillOrderWithProcedureAsync(int idWarehouse, int idProduct, int amount, DateTime requestDateTime)
    {
        return await _warehouseRepository.FulfillOrderWithProcedureAsync(idWarehouse, idProduct, amount, requestDateTime);
    }
    


    public async Task<int> FulfillOrderAsync(int idWarehouse, int idProduct, int amount,DateTime requestDateTime)
    {
        Product? product = await _warehouseRepository.GetProductAsync(idProduct);
        if (product == null)
        {
            throw new NoSuchProductException();
        }

        double price = product.Price;
        if (await _warehouseRepository.GetWarehouseAsync(idWarehouse) == null)
        {
            throw new NoSuchWarehouseException();
        }

        if (amount <= 0)
        {
            throw new AmountZeroOrLowerException();
        }

        Order? orderToFulfill = await _warehouseRepository.GetUnfulfilledOrderAsync(idProduct, amount);
        if (orderToFulfill == null)
        {
            throw new NoMatchingOrderException();
        }
        
        
        //if date in the request is earlier than the order date
        if ( orderToFulfill.CreatedAt.CompareTo(requestDateTime) >= 0)
        {
            throw new NoMatchingOrderException();
        }

        if (await _warehouseRepository.CheckIfProductWarehouseExists(orderToFulfill.IdOrder))
        {
            throw new OrderAlreadyFulfilledException();
        }
        //int idWarehouse, int idProduct, int idOrder, int amount, double price
        return await _warehouseRepository.FulfillOrderAsync(idWarehouse, idProduct, orderToFulfill.IdOrder, amount,price);

    }
}