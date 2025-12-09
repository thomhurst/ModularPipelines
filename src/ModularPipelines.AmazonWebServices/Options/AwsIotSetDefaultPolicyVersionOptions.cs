using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "set-default-policy-version")]
public record AwsIotSetDefaultPolicyVersionOptions(
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--policy-version-id")] string PolicyVersionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}