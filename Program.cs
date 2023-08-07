using Camunda.Api.Client;
using Camunda.Api.Client.ExternalTask;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AmarraderoLlanero.Data;
using AmarraderoLlanero.Models;
using AmarraderoLlanero.Functions;

var builder = WebApplication.CreateBuilder(args);

//Postgres Database Conection
builder.Services.AddNpgsql<AmarraderoLlaneroContext>(builder.Configuration.GetConnectionString("PostgreSQLConnection"));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapGet("/", () => Console.Write("Hello World!"));

app.MapGet("/dbconexion", async ([FromServices] AmarraderoLlaneroContext dbcontext) =>
{
    dbcontext.Database.EnsureCreated();
    return Results.Ok("Base de datos creada Crack");
});

CamundaClient camunda = CamundaClient.Create("http://camunda:8080/engine-rest");
//ejecutar cada 5 segundos

var timer = new System.Timers.Timer(TimeSpan.FromSeconds(10)); // se ejecutara cada 2 segundos
timer.Elapsed += async (sender, e) =>
{

    List<ExternalTaskInfo> allTask = await camunda.ExternalTasks.Query().List();
    AmarraderoLlaneroContext dbcontext = new AmarraderoLlaneroContext(builder.Services.BuildServiceProvider().GetService<DbContextOptions<AmarraderoLlaneroContext>>());

    foreach (ExternalTaskInfo task in allTask)
    {
        var nameTask = task.TopicName;

        switch (nameTask)
        {
            case "CreateUser":
                await UserFunctions.CreateUser(task, dbcontext, camunda);
                break;
            case "ValidateCredentials":
                await UserFunctions.ValidateCredentials(task, dbcontext, camunda);
                break;
            case "runOrder":
                await OrderFunctions.CreateOrder(task, dbcontext, camunda);
                break;
            default:
                break;
        }

    }

};
timer.Start(); // indicamos que unicie

app.Run();
