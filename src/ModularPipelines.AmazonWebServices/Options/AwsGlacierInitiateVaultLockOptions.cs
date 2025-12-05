using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glacier", "initiate-vault-lock")]
public record AwsGlacierInitiateVaultLockOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--vault-name")] string VaultName
) : AwsOptions
{
    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}