using System.Net;
using System.Reflection;
using DbUp;
using RealtimeRating.Composition;
using RealtimeRating.CustomerDomain;
using RealtimeRating.CustomerQuoteDomain;
using RealtimeRating.PolicyLineDefinitionDomain;
using RealtimeRating.QuoteDomain;
using RealtimeRating.RatingDomain;

const string policyLineDefinitionsDbConnectionString = "Server=.\\SqlServer2017;Database=OrleansPolicyLineDefinitions;Trusted_Connection=True;Pooling=True;Encrypt=false";
const string quotesDbConnectionString = "Server=.\\SqlServer2017;Database=OrleansQuotes;Trusted_Connection=True;Pooling=True;Encrypt=false";
const string ratingDbConnectionString = "Server=.\\SqlServer2017;Database=OrleansRating;Trusted_Connection=True;Pooling=True;Encrypt=false";
const string customerQuoteDbConnectionString = "Server=.\\SqlServer2017;Database=OrleansCustomerQuote;Trusted_Connection=True;Pooling=True;Encrypt=false";

DoDbUp();

var builder = WebApplication.CreateBuilder(args);

const int defaultSiloPort = 11111;
var portSpread = args.Length < 1 ? 0 : int.Parse(args[0]);
var siloPort = defaultSiloPort + portSpread;
var gatewayPort = 30000 + portSpread;
var runPort = int.Parse("727" + portSpread);

Console.WriteLine();
Console.WriteLine("--------------------------------");
Console.WriteLine($"PORT SPREAD = {portSpread}");
Console.WriteLine($"SILO PORT = {siloPort}");
Console.WriteLine($"GATEWAY PORT = {gatewayPort}");
Console.WriteLine($"RUN PORT = {runPort}");
Console.WriteLine("--------------------------------");
Console.WriteLine();

// Add services to the container.

builder.Host.UseOrleans(siloBuilder =>
{
    if (builder.Environment.IsDevelopment())
    {
        siloBuilder
            .UseDevelopmentClustering(new IPEndPoint(IPAddress.Loopback, defaultSiloPort))
            .ConfigureEndpoints(IPAddress.Loopback, siloPort, gatewayPort);

        siloBuilder.UseDashboard(c => c.Port = 7269);
    }

    siloBuilder.AddMemoryGrainStorageAsDefault();

    //https://learn.microsoft.com/en-us/dotnet/orleans/host/configuration-guide/adonet-configuration
    siloBuilder.AddAdoNetGrainStorage(PolicyLineDefinitionDomainConstants.StorageName,
        configureOptions =>
        {
            configureOptions.Invariant = "System.Data.SqlClient";
            configureOptions.ConnectionString = policyLineDefinitionsDbConnectionString;
        });

    siloBuilder.AddAdoNetGrainStorage(QuoteDomainConstants.StorageName,
        configureOptions =>
        {
            configureOptions.Invariant = "System.Data.SqlClient";
            configureOptions.ConnectionString = quotesDbConnectionString;
        });

    siloBuilder.AddAdoNetGrainStorage(RatingDomainConstants.StorageName,
        configureOptions =>
        {
            configureOptions.Invariant = "System.Data.SqlClient";
            configureOptions.ConnectionString = ratingDbConnectionString;
        });

    siloBuilder.AddAdoNetGrainStorage(CustomerQuoteDomainConstants.StorageName,
        configureOptions =>
        {
            configureOptions.Invariant = "System.Data.SqlClient";
            configureOptions.ConnectionString = customerQuoteDbConnectionString;
        });
});

builder.Services.AddControllers();

builder
    .Services
    .AddGrainComposer()
    .AddCustomerDomain()
    .AddPolicyLineDefinitionDomain()
    .AddQuoteDomain()
    .AddRatingDomain()
    .AddCustomerQuoteDomain();

builder.Services.AddCors(
    setupAction =>
    {
        setupAction.AddDefaultPolicy(
            policy =>
            {
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowAnyOrigin();
            });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Logger.LogInformation("STARTING APPLICATION");

app.Run("https://localhost:" + runPort);

/****************************************/

static void DoDbUp()
{
    EnsureDatabase.For.SqlDatabase(policyLineDefinitionsDbConnectionString);
    DeployChanges
        .To
        .SqlDatabase(policyLineDefinitionsDbConnectionString)
        .WithScriptsEmbeddedInAssembly(Assembly.GetEntryAssembly())
        .LogToConsole()
        .Build()
        .PerformUpgrade();

    EnsureDatabase.For.SqlDatabase(quotesDbConnectionString);
    DeployChanges
        .To
        .SqlDatabase(quotesDbConnectionString)
        .WithScriptsEmbeddedInAssembly(Assembly.GetEntryAssembly())
        .LogToConsole()
        .Build()
        .PerformUpgrade();

    EnsureDatabase.For.SqlDatabase(ratingDbConnectionString);
    DeployChanges
        .To
        .SqlDatabase(ratingDbConnectionString)
        .WithScriptsEmbeddedInAssembly(Assembly.GetEntryAssembly())
        .LogToConsole()
        .Build()
        .PerformUpgrade();

    EnsureDatabase.For.SqlDatabase(customerQuoteDbConnectionString);
    DeployChanges
        .To
        .SqlDatabase(customerQuoteDbConnectionString)
        .WithScriptsEmbeddedInAssembly(Assembly.GetEntryAssembly())
        .LogToConsole()
        .Build()
        .PerformUpgrade();
}