using Application.Jobs;
using Hangfire;

namespace Presentation.Configurations;

public abstract class JobConfiguration
{
    public static void Configure()
    {
        RecurringJob.AddOrUpdate<ITokenCleanupJob>(
                "Remove tokens every 12 hours",
                x => x.RemoveRevokedTokens(), 
                "0 */12 * * *");
    }
}
