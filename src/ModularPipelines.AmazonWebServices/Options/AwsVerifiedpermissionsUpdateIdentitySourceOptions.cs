using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("verifiedpermissions", "update-identity-source")]
public record AwsVerifiedpermissionsUpdateIdentitySourceOptions(
[property: CommandSwitch("--policy-store-id")] string PolicyStoreId,
[property: CommandSwitch("--identity-source-id")] string IdentitySourceId,
[property: CommandSwitch("--update-configuration")] string UpdateConfiguration
) : AwsOptions
{
    [CommandSwitch("--principal-entity-type")]
    public string? PrincipalEntityType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}