using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "create-members")]
public record AwsGuarddutyCreateMembersOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--account-details")] string[] AccountDetails
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}