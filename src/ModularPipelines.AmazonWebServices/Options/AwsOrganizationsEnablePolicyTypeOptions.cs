using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("organizations", "enable-policy-type")]
public record AwsOrganizationsEnablePolicyTypeOptions(
[property: CliOption("--root-id")] string RootId,
[property: CliOption("--policy-type")] string PolicyType
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}