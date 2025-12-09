using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "get-lifecycle-policy")]
public record AwsImagebuilderGetLifecyclePolicyOptions(
[property: CliOption("--lifecycle-policy-arn")] string LifecyclePolicyArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}