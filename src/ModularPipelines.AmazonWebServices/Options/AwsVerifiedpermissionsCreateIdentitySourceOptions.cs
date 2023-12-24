using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("verifiedpermissions", "create-identity-source")]
public record AwsVerifiedpermissionsCreateIdentitySourceOptions(
[property: CommandSwitch("--policy-store-id")] string PolicyStoreId,
[property: CommandSwitch("--configuration")] string Configuration
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--principal-entity-type")]
    public string? PrincipalEntityType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}