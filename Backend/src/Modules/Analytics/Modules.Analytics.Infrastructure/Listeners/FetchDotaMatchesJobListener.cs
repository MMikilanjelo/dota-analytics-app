using Modules.Analytics.Infrastructure.Jobs;
using Quartz;

namespace Modules.Analytics.Infrastructure.Listeners;

public class FetchDotaMatchesJobListener(ISchedulerFactory schedulerFactory) : IJobListener
{
    public string Name => nameof(FetchDotaMatchesJobListener);

    public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default) => 
        Task.CompletedTask;

    public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default) => 
        Task.CompletedTask;

    public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException? jobException, CancellationToken cancellationToken = new CancellationToken())
    {
        var scheduler= await schedulerFactory.GetScheduler(cancellationToken);

        var trigger = TriggerBuilder.Create()
            .ForJob(ParseDotaMatchReplayJob.Key)
            .WithIdentity("parseDotaMatchReplayJob")
            .StartNow()
            .Build();

        await scheduler.ScheduleJob(trigger, cancellationToken);
    }
}