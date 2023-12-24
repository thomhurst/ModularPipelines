using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fms", "get-compliance-detail")]
public record AwsFmsGetComplianceDetailOptions(
[property: CommandSwitch("--policy-id")] string PolicyId,
[property: CommandSwitch("--member-account")] string MemberAccount
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}