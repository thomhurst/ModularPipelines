using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cleanrooms", "preview-privacy-impact")]
public record AwsCleanroomsPreviewPrivacyImpactOptions(
[property: CliOption("--membership-identifier")] string MembershipIdentifier,
[property: CliOption("--parameters")] string Parameters
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}