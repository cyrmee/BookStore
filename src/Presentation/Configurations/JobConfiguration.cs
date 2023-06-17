using Application.Jobs;
using Hangfire;

namespace Presentation.Configurations;

public abstract class JobConfiguration
{
    public static void Configure()
    {
        RecurringJob.AddOrUpdate<ITokenCleanupJob>(
                "Remove tokens at midnight, every day",
                x => x.RemoveRevokedTokens(), 
                "0 */12 * * *");
    }
}
