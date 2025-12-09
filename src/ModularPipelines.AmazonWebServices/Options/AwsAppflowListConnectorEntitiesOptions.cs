using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appflow", "list-connector-entities")]
public record AwsAppflowListConnectorEntitiesOptions : AwsOptions
{
    [CliOption("--connector-profile-name")]
    public string? ConnectorProfileName { get; set; }

    [CliOption("--connector-type")]
    public string? ConnectorType { get; set; }

    [CliOption("--entities-path")]
    public string? EntitiesPath { get; set; }

    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}