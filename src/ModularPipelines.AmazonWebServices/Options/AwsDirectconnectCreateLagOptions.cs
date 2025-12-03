using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "create-lag")]
public record AwsDirectconnectCreateLagOptions(
[property: CliOption("--number-of-connections")] int NumberOfConnections,
[property: CliOption("--location")] string Location,
[property: CliOption("--connections-bandwidth")] string ConnectionsBandwidth,
[property: CliOption("--lag-name")] string LagName
) : AwsOptions
{
    [CliOption("--connection-id")]
    public string? ConnectionId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--child-connection-tags")]
    public string[]? ChildConnectionTags { get; set; }

    [CliOption("--provider-name")]
    public string? ProviderName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}