using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "container-rm", "create")]
public record AzStorageContainerRmCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--storage-account")] int StorageAccount
) : AzOptions
{
    [CliOption("--default-encryption-scope")]
    public string? DefaultEncryptionScope { get; set; }

    [CliFlag("--deny-encryption-scope-override")]
    public bool? DenyEncryptionScopeOverride { get; set; }

    [CliFlag("--enable-vlw")]
    public bool? EnableVlw { get; set; }

    [CliFlag("--fail-on-exist")]
    public bool? FailOnExist { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliOption("--public-access")]
    public string? PublicAccess { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--root-squash")]
    public string? RootSquash { get; set; }
}