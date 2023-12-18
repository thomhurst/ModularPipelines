using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzNewRelic
{
    public AzNewRelic(
        AzNewRelicAccount account,
        AzNewRelicMonitor monitor,
        AzNewRelicOrganization organization,
        AzNewRelicPlan plan
    )
    {
        Account = account;
        Monitor = monitor;
        Organization = organization;
        Plan = plan;
    }

    public AzNewRelicAccount Account { get; }

    public AzNewRelicMonitor Monitor { get; }

    public AzNewRelicOrganization Organization { get; }

    public AzNewRelicPlan Plan { get; }
}