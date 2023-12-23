using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("grafana", "create-workspace-api-key")]
public record AwsGrafanaCreateWorkspaceApiKeyOptions(
[property: CommandSwitch("--key-name")] string KeyName,
[property: CommandSwitch("--key-role")] string KeyRole,
[property: CommandSwitch("--seconds-to-live")] int SecondsToLive,
[property: CommandSwitch("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}