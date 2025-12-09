using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fms", "get-compliance-detail")]
public record AwsFmsGetComplianceDetailOptions(
[property: CliOption("--policy-id")] string PolicyId,
[property: CliOption("--member-account")] string MemberAccount
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}