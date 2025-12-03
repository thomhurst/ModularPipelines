using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appflow", "reset-connector-metadata-cache")]
public record AwsAppflowResetConnectorMetadataCacheOptions : AwsOptions
{
    [CliOption("--connector-profile-name")]
    public string? ConnectorProfileName { get; set; }

    [CliOption("--connector-type")]
    public string? ConnectorType { get; set; }

    [CliOption("--connector-entity-name")]
    public string? ConnectorEntityName { get; set; }

    [CliOption("--entities-path")]
    public string? EntitiesPath { get; set; }

    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}