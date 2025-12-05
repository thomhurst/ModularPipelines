using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glacier", "set-vault-access-policy")]
public record AwsGlacierSetVaultAccessPolicyOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--vault-name")] string VaultName
) : AwsOptions
{
    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}