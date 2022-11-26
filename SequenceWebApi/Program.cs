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
builder.Services.AddSingleton(x => new TemplateSequence(x.GetRequiredService<ISequenceConfiguration>().SequenceTemplate, x.GetRequiredService<ISequenceRepository>()));
builder.Services.AddSingleton(x => new NaturalNumbersSequence(x.GetRequiredService<ISequenceConfiguration>().SequenceNaturalNumbers, x.GetRequiredService<ISequenceRepository>()));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.Logger.LogInformation("Started");

app.MapGet("/naturalNumberCurrent", () =>
{
    app.Logger.LogInformation("naturalNumberCurrent request");
    return app.Services.GetRequiredService<NaturalNumbersSequence>().GetCurrent();
})
.WithName("GetCurrentNaturalNumber request");

app.MapGet("/naturalNumberNext", () =>
{
    app.Logger.LogInformation("naturalNumberNext request");
    return app.Services.GetRequiredService<NaturalNumbersSequence>().GetNext();
})
.WithName("GetNextNaturalNumber");

app.MapGet("/templateCurrent", () =>
{
    app.Logger.LogInformation("templateCurrent request");
    return app.Services.GetRequiredService<TemplateSequence>().GetCurrent();
})
.WithName("GetCurrentTemplate");

app.MapGet("/templateNext", () =>
{
    app.Logger.LogInformation("templateNext request");
    return app.Services.GetRequiredService<TemplateSequence>().GetNext();
})
.WithName("GetNextTemplate");

app.Run();