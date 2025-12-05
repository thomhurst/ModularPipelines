using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "describe-loa")]
public record AwsDirectconnectDescribeLoaOptions(
[property: CliOption("--connection-id")] string ConnectionId
) : AwsOptions
{
    [CliOption("--provider-name")]
    public string? ProviderName { get; set; }

    [CliOption("--loa-content-type")]
    public string? LoaContentType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}