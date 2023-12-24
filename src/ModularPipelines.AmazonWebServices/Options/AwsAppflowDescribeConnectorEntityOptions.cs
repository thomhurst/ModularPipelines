using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appflow", "describe-connector-entity")]
public record AwsAppflowDescribeConnectorEntityOptions(
[property: CommandSwitch("--connector-entity-name")] string ConnectorEntityName
) : AwsOptions
{
    [CommandSwitch("--connector-type")]
    public string? ConnectorType { get; set; }

    [CommandSwitch("--connector-profile-name")]
    public string? ConnectorProfileName { get; set; }

    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}