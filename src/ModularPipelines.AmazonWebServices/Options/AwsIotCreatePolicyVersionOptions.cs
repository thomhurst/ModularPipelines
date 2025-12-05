using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-policy-version")]
public record AwsIotCreatePolicyVersionOptions(
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--policy-document")] string PolicyDocument
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}