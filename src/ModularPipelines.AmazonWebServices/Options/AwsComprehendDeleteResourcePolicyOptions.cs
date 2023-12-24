using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "delete-resource-policy")]
public record AwsComprehendDeleteResourcePolicyOptions(
[property: CommandSwitch("--resource-arn")] string ResourceArn
) : AwsOptions
{
    [CommandSwitch("--policy-revision-id")]
    public string? PolicyRevisionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}