using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securitylake", "create-data-lake-organization-configuration")]
public record AwsSecuritylakeCreateDataLakeOrganizationConfigurationOptions(
[property: CommandSwitch("--auto-enable-new-account")] string[] AutoEnableNewAccount
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}