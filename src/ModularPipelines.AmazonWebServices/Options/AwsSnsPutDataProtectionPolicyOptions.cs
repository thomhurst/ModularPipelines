using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sns", "put-data-protection-policy")]
public record AwsSnsPutDataProtectionPolicyOptions(
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--data-protection-policy")] string DataProtectionPolicy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}