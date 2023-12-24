using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime", "batch-update-user")]
public record AwsChimeBatchUpdateUserOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--update-user-request-items")] string[] UpdateUserRequestItems
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}