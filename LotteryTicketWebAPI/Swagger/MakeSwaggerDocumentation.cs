namespace LotteryTicketWebAPI.Swagger;

internal static class MakeSwaggerDocumentation
{
    internal static void CreateSwaggerDocumentation(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen();

        WebApplication app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}