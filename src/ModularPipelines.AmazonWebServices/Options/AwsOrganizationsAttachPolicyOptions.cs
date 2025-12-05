using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("organizations", "attach-policy")]
public record AwsOrganizationsAttachPolicyOptions(
[property: CliOption("--policy-id")] string PolicyId,
[property: CliOption("--target-id")] string TargetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}