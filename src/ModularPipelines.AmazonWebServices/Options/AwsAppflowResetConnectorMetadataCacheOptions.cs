using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appflow", "reset-connector-metadata-cache")]
public record AwsAppflowResetConnectorMetadataCacheOptions : AwsOptions
{
    [CommandSwitch("--connector-profile-name")]
    public string? ConnectorProfileName { get; set; }

    [CommandSwitch("--connector-type")]
    public string? ConnectorType { get; set; }

    [CommandSwitch("--connector-entity-name")]
    public string? ConnectorEntityName { get; set; }

    [CommandSwitch("--entities-path")]
    public string? EntitiesPath { get; set; }

    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}