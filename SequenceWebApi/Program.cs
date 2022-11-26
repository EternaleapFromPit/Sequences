using Microsoft.Extensions.DependencyInjection;
using SequenceLibrary.Configuration;
using SequenceLibrary.Repository;
using SequenceLibrary.Sequences;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//SequenceLibrary.dll.config has been copied.
//The reason for that is following - library could be used by different servers (grpc, REST API, sockets, used directly)
builder.Services.AddSingleton<ISequenceConfiguration, SequenceConfiguration>();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<ISequenceRepository, MsSqlSequenceRepository>();
builder.Services.AddSingleton(x => new TemplateSequence(x.GetService<ISequenceConfiguration>().SequenceTemplate, x.GetService<ISequenceRepository>()));
builder.Services.AddSingleton(x => new NaturalNumbersSequence(x.GetService<ISequenceConfiguration>().SequenceNaturalNumbers, x.GetService<ISequenceRepository>()));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/naturalNumberCurrent", () =>
{
    Console.WriteLine("naturalNumberCurrent");
    return app.Services.GetService<NaturalNumbersSequence>().GetCurrent();
})
.WithName("GetCurrentNaturalNumber");

app.MapGet("/naturalNumberNext", () =>
{
    Console.WriteLine("naturalNumberNext");
    return app.Services.GetService<NaturalNumbersSequence>().GetNext();
})
.WithName("GetNextNaturalNumber");

app.MapGet("/templateCurrent", () =>
{
    Console.WriteLine("templateCurrent");
    return app.Services.GetService<TemplateSequence>().GetCurrent();
})
.WithName("GetCurrentTemplate");

app.MapGet("/templateNext", () =>
{
    Console.WriteLine("templateNext");
    return app.Services.GetService<TemplateSequence>().GetNext();
})
.WithName("GetNextTemplate");

app.Run();
