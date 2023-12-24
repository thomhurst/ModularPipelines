using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "detach-policy")]
public record AwsIotDetachPolicyOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--target")] string Target
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}