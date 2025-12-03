using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "get-policy-version")]
public record AwsIotGetPolicyVersionOptions(
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--policy-version-id")] string PolicyVersionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}