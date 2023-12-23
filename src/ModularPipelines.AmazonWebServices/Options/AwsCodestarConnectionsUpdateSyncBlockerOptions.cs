using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar-connections", "update-sync-blocker")]
public record AwsCodestarConnectionsUpdateSyncBlockerOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--sync-type")] string SyncType,
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--resolved-reason")] string ResolvedReason
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}