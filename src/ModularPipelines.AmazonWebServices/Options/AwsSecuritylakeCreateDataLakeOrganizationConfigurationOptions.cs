using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("securitylake", "create-data-lake-organization-configuration")]
public record AwsSecuritylakeCreateDataLakeOrganizationConfigurationOptions(
[property: CliOption("--auto-enable-new-account")] string[] AutoEnableNewAccount
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}