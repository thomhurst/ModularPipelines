using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securityhub", "update-organization-configuration")]
public record AwsSecurityhubUpdateOrganizationConfigurationOptions : AwsOptions
{
    [CliOption("--auto-enable-standards")]
    public string? AutoEnableStandards { get; set; }

    [CliOption("--organization-configuration")]
    public string? OrganizationConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}