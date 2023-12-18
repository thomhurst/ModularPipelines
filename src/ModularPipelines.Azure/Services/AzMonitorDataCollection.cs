using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor")]
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

