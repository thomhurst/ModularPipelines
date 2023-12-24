using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("accessanalyzer", "check-no-new-access")]
public record AwsAccessanalyzerCheckNoNewAccessOptions(
[property: CommandSwitch("--new-policy-document")] string NewPolicyDocument,
[property: CommandSwitch("--existing-policy-document")] string ExistingPolicyDocument,
[property: CommandSwitch("--policy-type")] string PolicyType
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}