using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "put-resource-policy")]
public record AwsGluePutResourcePolicyOptions(
[property: CommandSwitch("--policy-in-json")] string PolicyInJson
) : AwsOptions
{
    [CommandSwitch("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CommandSwitch("--policy-hash-condition")]
    public string? PolicyHashCondition { get; set; }

    [CommandSwitch("--policy-exists-condition")]
    public string? PolicyExistsCondition { get; set; }

    [CommandSwitch("--enable-hybrid")]
    public string? EnableHybrid { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}