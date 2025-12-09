using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "put-account-policy")]
public record AwsLogsPutAccountPolicyOptions(
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--policy-document")] string PolicyDocument,
[property: CliOption("--policy-type")] string PolicyType
) : AwsOptions
{
    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}