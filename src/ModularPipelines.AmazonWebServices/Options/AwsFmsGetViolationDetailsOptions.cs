using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fms", "get-violation-details")]
public record AwsFmsGetViolationDetailsOptions(
[property: CliOption("--policy-id")] string PolicyId,
[property: CliOption("--member-account")] string MemberAccount,
[property: CliOption("--resource-id")] string ResourceId,
[property: CliOption("--resource-type")] string ResourceType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}