using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("shield", "update-emergency-contact-settings")]
public record AwsShieldUpdateEmergencyContactSettingsOptions : AwsOptions
{
    [CliOption("--emergency-contact-list")]
    public string[]? EmergencyContactList { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}