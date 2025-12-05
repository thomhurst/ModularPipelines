using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("grafana", "delete-workspace")]
public record AwsGrafanaDeleteWorkspaceOptions(
[property: CliOption("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}