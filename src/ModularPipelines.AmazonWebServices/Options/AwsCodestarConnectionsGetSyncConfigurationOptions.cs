using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar-connections", "get-sync-configuration")]
public record AwsCodestarConnectionsGetSyncConfigurationOptions(
[property: CommandSwitch("--sync-type")] string SyncType,
[property: CommandSwitch("--resource-name")] string ResourceName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}