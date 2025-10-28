using Quartz;

namespace Modules.Analytics.Infrastructure.Jobs;

[DisallowConcurrentExecution]
public class ParseDotaMatchReplayJob : IJob
{
    public static readonly JobKey Key = JobKey.Create(nameof(ParseDotaMatchReplayJob));

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("ParseDotaMatchReplayJob");
    }
}