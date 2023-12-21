using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "eventhub", "consumer-group", "update")]
public record AzEventhubsEventhubConsumerGroupUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--consumer-group-name")]
    public string? ConsumerGroupName { get; set; }

    [CommandSwitch("--eventhub-name")]
    public string? EventhubName { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--namespace-name")]
    public string? NamespaceName { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--user-metadata")]
    public string? UserMetadata { get; set; }
}