using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor")]
public class AzMonitorDataCollection
{
    public AzMonitorDataCollection(
        AzMonitorDataCollectionEndpoint endpoint,
        AzMonitorDataCollectionRule rule
    )
    {
        Endpoint = endpoint;
        Rule = rule;
    }

    public AzMonitorDataCollectionEndpoint Endpoint { get; }

    public AzMonitorDataCollectionRule Rule { get; }
}