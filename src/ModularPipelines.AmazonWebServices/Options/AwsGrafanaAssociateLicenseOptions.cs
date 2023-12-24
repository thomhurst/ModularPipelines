using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "associate-license")]
public record AwsGrafanaAssociateLicenseOptions(
[property: CommandSwitch("--license-type")] string LicenseType,
[property: CommandSwitch("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}