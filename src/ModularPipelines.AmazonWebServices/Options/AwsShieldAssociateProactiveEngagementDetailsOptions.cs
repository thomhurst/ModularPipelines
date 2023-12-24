using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("shield", "associate-proactive-engagement-details")]
public record AwsShieldAssociateProactiveEngagementDetailsOptions(
[property: CommandSwitch("--emergency-contact-list")] string[] EmergencyContactList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}