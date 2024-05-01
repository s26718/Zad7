using System.Data.SqlClient;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Zad7.Exceptions;
using Zad7.Models;
using Zad7.Repositories;
using Zad7.Services;

namespace Zad7;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    
    private IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }
    


    [HttpPost("zad1")]
    public async Task<IActionResult> FulfillOrderAsync(RequestDTO requestDto)
    {
        int? productWarehouseId = null;
        (int idProduct, int idWarehouse, int amount, DateTime requestDateTime) = requestDto;
        try
        {
            productWarehouseId =
                await _warehouseService.FulfillOrderAsync(idWarehouse, idProduct, amount, requestDateTime);
        }
        catch (NoSuchProductException)
        {
            return StatusCode(404, "No such product exists");
        }
        catch (NoSuchWarehouseException)
        {
            return StatusCode(404, "No such warehouse exists");
        }
        catch (AmountZeroOrLowerException)
        {
            return StatusCode(400, "amount must be greater than zero");
        }
        catch (NoMatchingOrderException)
        {
            return StatusCode(404, "no matching order found");
        }
        catch (OrderAlreadyFulfilledException)
        {
            return StatusCode(400, "order already fulfilled");
        }
        

        return Ok(productWarehouseId);
    }
    [HttpPost("zad2")]
    public async Task<IActionResult> FulfillOrderWithProcedureAsync(RequestDTO requestDto)
    {
        int? productWarehouseId = null;
        (int idProduct, int idWarehouse, int amount, DateTime requestDateTime) = requestDto;
        try
        {
            productWarehouseId =
                await _warehouseService.FulfillOrderWithProcedureAsync(idWarehouse, idProduct, amount, requestDateTime);
        }
        catch (SqlException exc)
        {
            switch (exc.State)
            {
                case 1:
                    return StatusCode(404, "No such product exists");
                    break;
                case 2:
                    return StatusCode(404, "no matching order found");
                    break;
                case 3:
                    return StatusCode(404, "No such warehouse exists");
                    break;
            }
        }


        return Ok(productWarehouseId);
    }
    


}