using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "create-connection")]
public record AwsDirectconnectCreateConnectionOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--bandwidth")] string Bandwidth,
[property: CliOption("--connection-name")] string ConnectionName
) : AwsOptions
{
    [CliOption("--lag-id")]
    public string? LagId { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--provider-name")]
    public string? ProviderName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}