using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glacier", "wait", "vault-not-exists")]
public record AwsGlacierWaitVaultNotExistsOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--vault-name")] string VaultName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}