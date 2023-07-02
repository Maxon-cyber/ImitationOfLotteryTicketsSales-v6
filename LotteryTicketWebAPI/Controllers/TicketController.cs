using LotteryTicketWebAPI.RequestProcessing;
using LotteryTicketWebAPI.ResponseToTheClientFromTheServer;
using Microsoft.AspNetCore.Mvc;

namespace LotteryTicketWebAPI.Controllers;

[Route("Ticket")]
public class TicketController : Controller
{
    [Route("/")]
    public ActionResult<string> GetAllTickets()
    {
        return "Tickets";
    }

    [HttpGet("GetTicketAnId/{id:int}")]
    public async Task<ActionResult<string>> Get(int? id)
    {
        if (id is null) 
            return await Task.FromResult<ActionResult<string>>(ResponseOnSite.BadRequest("¬ведите id"));
        
        await new ProcessingClientRequests("Get", id).SendDataClientToServerAsync();

        if (ProcessingClientRequests.Response is null) 
            return await Task.FromResult<ActionResult<string>>(ResponseOnSite.NotFound("Ѕилет не найден"));

        return await Task.FromResult<ActionResult<string>>(ResponseOnSite.Ok(ProcessingClientRequests.Response));
    }

    [HttpPost("BuyTicketAnId/{id:int}")]
    public async Task<ActionResult<string>> Post(int? id)
    {
        if (id is null) 
            return await Task.FromResult<ActionResult<string>>(ResponseOnSite.BadRequest("¬ведите id"));

        await new ProcessingClientRequests("Post", id).SendDataClientToServerAsync();

        if (ProcessingClientRequests.Response is null)
            return await Task.FromResult<ActionResult<string>>(ResponseOnSite.NotFound("Ѕилет не найден"));

        return await Task.FromResult<ActionResult<string>>(ResponseOnSite.Ok(ProcessingClientRequests.Response));
    }
}