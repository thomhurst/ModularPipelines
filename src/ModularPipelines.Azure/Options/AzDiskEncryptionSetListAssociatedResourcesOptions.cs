using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("disk-encryption-set", "list-associated-resources")]
public record AzDiskEncryptionSetListAssociatedResourcesOptions(
[property: CliOption("--disk-encryption-set-name")] string DiskEncryptionSetName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }
}