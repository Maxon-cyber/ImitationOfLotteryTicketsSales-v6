using LotteryTicketWebAPI.RequestProcessing;
using LotteryTicketWebAPI.ResponseToTheClientFromTheServer;
using Microsoft.AspNetCore.Mvc;

namespace LotteryTicketWebAPI.Controllers;

[Route("Ticket")]
public class TicketController : Controller
{
    [HttpGet("GetAllTickets")]
    public async Task<ActionResult<string>> GetAllTickets()
    {
        await new ProcessingClientRequests("GetAllTickets", null).SendDataClientToServerAsync();

        if (ProcessingClientRequests.Response is "null")
            return await Task.FromResult<ActionResult<string>>(ResponseOnSite.NotFound("����� �� ������"));

        return await Task.FromResult<ActionResult<string>>(ResponseOnSite.Ok(ProcessingClientRequests.Response));
    }

    [HttpGet("GetTicket/{id:int?}")]
    public async Task<ActionResult<string>> GetTicket(long? id)
    {
        if (id is null) 
            return await Task.FromResult<ActionResult<string>>(ResponseOnSite.BadRequest("������� id"));
        
        await new ProcessingClientRequests("GetTicketAnId", id).SendDataClientToServerAsync();

        if (ProcessingClientRequests.Response is "null") 
            return await Task.FromResult<ActionResult<string>>(ResponseOnSite.NotFound("����� �� ������"));

        return await Task.FromResult<ActionResult<string>>(ResponseOnSite.Ok(ProcessingClientRequests.Response));
    }

    [HttpPost("BuyTicket/{id:int?}")]
    public async Task<ActionResult<string>> BuyTicket(long? id)
    {
        if (id is null) 
            return await Task.FromResult<ActionResult<string>>(ResponseOnSite.BadRequest("������� id"));

        await new ProcessingClientRequests("BuyTicketAnId", id).SendDataClientToServerAsync();

        if (ProcessingClientRequests.Response is "null")
            return await Task.FromResult<ActionResult<string>>(ResponseOnSite.NotFound("����� �� ������"));

        return await Task.FromResult<ActionResult<string>>(ResponseOnSite.Ok(ProcessingClientRequests.Response));
    }
}