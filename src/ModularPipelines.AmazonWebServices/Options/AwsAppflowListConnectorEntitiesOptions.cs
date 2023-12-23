using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appflow", "list-connector-entities")]
public record AwsAppflowListConnectorEntitiesOptions : AwsOptions
{
    [CommandSwitch("--connector-profile-name")]
    public string? ConnectorProfileName { get; set; }

    [CommandSwitch("--connector-type")]
    public string? ConnectorType { get; set; }

    [CommandSwitch("--entities-path")]
    public string? EntitiesPath { get; set; }

    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}