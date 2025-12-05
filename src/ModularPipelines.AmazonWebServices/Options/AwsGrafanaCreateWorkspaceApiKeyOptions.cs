using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("grafana", "create-workspace-api-key")]
public record AwsGrafanaCreateWorkspaceApiKeyOptions(
[property: CliOption("--key-name")] string KeyName,
[property: CliOption("--key-role")] string KeyRole,
[property: CliOption("--seconds-to-live")] int SecondsToLive,
[property: CliOption("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}