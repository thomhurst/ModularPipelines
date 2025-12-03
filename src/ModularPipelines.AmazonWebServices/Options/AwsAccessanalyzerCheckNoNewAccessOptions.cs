using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("accessanalyzer", "check-no-new-access")]
public record AwsAccessanalyzerCheckNoNewAccessOptions(
[property: CliOption("--new-policy-document")] string NewPolicyDocument,
[property: CliOption("--existing-policy-document")] string ExistingPolicyDocument,
[property: CliOption("--policy-type")] string PolicyType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}