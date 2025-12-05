using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securitylake", "delete-data-lake-organization-configuration")]
public record AwsSecuritylakeDeleteDataLakeOrganizationConfigurationOptions(
[property: CliOption("--auto-enable-new-account")] string[] AutoEnableNewAccount
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}