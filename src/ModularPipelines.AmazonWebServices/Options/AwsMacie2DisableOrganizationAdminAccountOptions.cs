using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("macie2", "disable-organization-admin-account")]
public record AwsMacie2DisableOrganizationAdminAccountOptions(
[property: CommandSwitch("--admin-account-id")] string AdminAccountId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}