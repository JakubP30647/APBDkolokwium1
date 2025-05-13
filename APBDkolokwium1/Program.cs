var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
//builder.Services.AddScoped<IVisitService, VisitService>();

builder.Services.AddOpenApi();
var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();