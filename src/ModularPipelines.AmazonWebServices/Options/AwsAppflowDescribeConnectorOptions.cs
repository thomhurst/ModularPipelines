using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appflow", "describe-connector")]
public record AwsAppflowDescribeConnectorOptions(
[property: CliOption("--connector-type")] string ConnectorType
) : AwsOptions
{
    [CliOption("--connector-label")]
    public string? ConnectorLabel { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}