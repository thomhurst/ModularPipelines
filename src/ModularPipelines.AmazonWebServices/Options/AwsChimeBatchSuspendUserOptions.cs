using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "batch-suspend-user")]
public record AwsChimeBatchSuspendUserOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--user-id-list")] string[] UserIdList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}