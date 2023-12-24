using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "put-account-policy")]
public record AwsLogsPutAccountPolicyOptions(
[property: CommandSwitch("--policy-name")] string PolicyName,
[property: CommandSwitch("--policy-document")] string PolicyDocument,
[property: CommandSwitch("--policy-type")] string PolicyType
) : AwsOptions
{
    [CommandSwitch("--scope")]
    public string? Scope { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}