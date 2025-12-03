using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appflow", "describe-connector-entity")]
public record AwsAppflowDescribeConnectorEntityOptions(
[property: CliOption("--connector-entity-name")] string ConnectorEntityName
) : AwsOptions
{
    [CliOption("--connector-type")]
    public string? ConnectorType { get; set; }

    [CliOption("--connector-profile-name")]
    public string? ConnectorProfileName { get; set; }

    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}