using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "update-organization-configuration")]
public record AwsSecurityhubUpdateOrganizationConfigurationOptions : AwsOptions
{
    [CommandSwitch("--auto-enable-standards")]
    public string? AutoEnableStandards { get; set; }

    [CommandSwitch("--organization-configuration")]
    public string? OrganizationConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}