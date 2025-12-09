using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appflow", "describe-connector-profiles")]
public record AwsAppflowDescribeConnectorProfilesOptions : AwsOptions
{
    [CliOption("--connector-profile-names")]
    public string[]? ConnectorProfileNames { get; set; }

    [CliOption("--connector-type")]
    public string? ConnectorType { get; set; }

    [CliOption("--connector-label")]
    public string? ConnectorLabel { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}