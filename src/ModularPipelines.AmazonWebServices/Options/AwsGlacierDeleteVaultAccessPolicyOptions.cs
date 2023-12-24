using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glacier", "delete-vault-access-policy")]
public record AwsGlacierDeleteVaultAccessPolicyOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--vault-name")] string VaultName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}