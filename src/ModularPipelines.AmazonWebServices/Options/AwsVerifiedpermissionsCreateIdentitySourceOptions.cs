using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("verifiedpermissions", "create-identity-source")]
public record AwsVerifiedpermissionsCreateIdentitySourceOptions(
[property: CliOption("--policy-store-id")] string PolicyStoreId,
[property: CliOption("--configuration")] string Configuration
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--principal-entity-type")]
    public string? PrincipalEntityType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}