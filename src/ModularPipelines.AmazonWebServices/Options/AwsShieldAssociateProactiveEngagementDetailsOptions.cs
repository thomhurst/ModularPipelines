using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("shield", "associate-proactive-engagement-details")]
public record AwsShieldAssociateProactiveEngagementDetailsOptions(
[property: CliOption("--emergency-contact-list")] string[] EmergencyContactList
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}