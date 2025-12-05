using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "describe-connections")]
public record AwsDirectconnectDescribeConnectionsOptions : AwsOptions
{
    [CliOption("--connection-id")]
    public string? ConnectionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}