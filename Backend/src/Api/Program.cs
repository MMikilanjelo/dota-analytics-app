using Common;
using Common.Contracts.Events;
using Modules.Analytics.Infrastructure;
using Modules.Analytics.Infrastructure.Extensions;
using Modules.ExternalAccounts.Api;
using Modules.ExternalAccounts.Application;
using Modules.ExternalAccounts.Infrastructure;
using Modules.ExternalAccounts.Infrastructure.Extensions;
using Modules.Users.Application;
using Modules.Users.Infrastructure;
using Modules.Users.Infrastructure.Extensions;
using Serilog;
using StrictId.HotChocolate;

var builder = WebApplication.CreateBuilder(args);
if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load();
}

builder.Configuration.AddEnvironmentVariables();

builder.Host.UseSerilog((_, loggerConfig) => { loggerConfig.WriteTo.Console(); });
builder.WebHost.UseUrls("http://0.0.0.0:5095");
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder
    .AddGraphQL()
    .AddStrictId()
    .AddQueryConventions()
    .AddMutationConventions(
        new MutationConventionOptions
        {
            InputArgumentName = "input",
            InputTypeNamePattern = "{MutationName}Input",
            PayloadTypeNamePattern = "{MutationName}Payload",
            PayloadErrorTypeNamePattern = "{MutationName}Error",
            PayloadErrorsFieldName = "errors",
            ApplyToAllMutations = true
        })
    .AddGlobalObjectIdentification()
    .AddTypes();

builder.Services
    .AddUsersModuleApplication()
    .AddUsersModuleInfrastructure(builder.Configuration)
    .AddExternalAccountsModuleApplication()
    .AddExternalAccountsModuleInfrastructure(builder.Configuration)
    .AddAnalyticsModuleInfrastructure(builder.Configuration)
    .DecorateModules();

builder.Services.AddScoped<IEventPublisher, EventPublisher>();


var app = builder.Build();

app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapSteamEndpoints();

app
    .ApplyUsersModuleMigrations()
    .ApplyExternalAccountsModuleMigrations()
    .ApplyAnalyticsModuleMigrations();

app.MapGraphQL();

app.RunWithGraphQLCommands(args);