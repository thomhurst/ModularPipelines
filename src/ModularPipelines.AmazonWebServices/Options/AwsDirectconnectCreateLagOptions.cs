using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "create-lag")]
public record AwsDirectconnectCreateLagOptions(
[property: CommandSwitch("--number-of-connections")] int NumberOfConnections,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--connections-bandwidth")] string ConnectionsBandwidth,
[property: CommandSwitch("--lag-name")] string LagName
) : AwsOptions
{
    [CommandSwitch("--connection-id")]
    public string? ConnectionId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--child-connection-tags")]
    public string[]? ChildConnectionTags { get; set; }

    [CommandSwitch("--provider-name")]
    public string? ProviderName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}