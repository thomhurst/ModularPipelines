using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("organizations", "deregister-delegated-administrator")]
public record AwsOrganizationsDeregisterDelegatedAdministratorOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--service-principal")] string ServicePrincipal
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}