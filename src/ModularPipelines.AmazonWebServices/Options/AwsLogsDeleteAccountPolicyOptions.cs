using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "delete-account-policy")]
public record AwsLogsDeleteAccountPolicyOptions(
[property: CliOption("--policy-name")] string PolicyName,
[property: CliOption("--policy-type")] string PolicyType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}