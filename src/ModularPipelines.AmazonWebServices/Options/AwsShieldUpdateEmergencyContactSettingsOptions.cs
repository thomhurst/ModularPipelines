using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("shield", "update-emergency-contact-settings")]
public record AwsShieldUpdateEmergencyContactSettingsOptions : AwsOptions
{
    [CommandSwitch("--emergency-contact-list")]
    public string[]? EmergencyContactList { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}