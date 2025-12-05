using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("shield", "describe-protection")]
public record AwsShieldDescribeProtectionOptions : AwsOptions
{
    [CliOption("--protection-id")]
    public string? ProtectionId { get; set; }

    [CliOption("--resource-arn")]
    public string? ResourceArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}