using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar-connections", "get-resource-sync-status")]
public record AwsCodestarConnectionsGetResourceSyncStatusOptions(
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--sync-type")] string SyncType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}