using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("organizations", "register-delegated-administrator")]
public record AwsOrganizationsRegisterDelegatedAdministratorOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--service-principal")] string ServicePrincipal
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}