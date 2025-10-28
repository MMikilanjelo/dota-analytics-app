using Application;
using SharedKernel;
using SharedKernel.Contracts.Messaging;
using Infrastructure;
using Modules.Analytics.Infrastructure;
using Modules.Analytics.Infrastructure.Extensions;
using Modules.ExternalAccounts.Api;
using Modules.ExternalAccounts.Application;
using Modules.ExternalAccounts.Infrastructure;
using Modules.ExternalAccounts.Infrastructure.Extensions;
using Modules.Users.Application;
using Modules.Users.Infrastructure;
using Modules.Users.Infrastructure.Extensions;
using Quartz;
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
    .AddCommonInfrastructure()
    .AddBehaviours()
    .AddQuartz(q =>
        {
            q.AddUsersJobs();
            q.AddAnalyticsJobs();
        }
    )
    .AddQuartzHostedService(opt => { opt.WaitForJobsToComplete = true; });

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