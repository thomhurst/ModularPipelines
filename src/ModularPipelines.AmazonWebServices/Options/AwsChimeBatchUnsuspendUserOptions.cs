using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "batch-unsuspend-user")]
public record AwsChimeBatchUnsuspendUserOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--user-id-list")] string[] UserIdList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}