using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "container", "create")]
public record AzStorageContainerCreateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--default-encryption-scope")]
    public string? DefaultEncryptionScope { get; set; }

    [CliFlag("--fail-on-exist")]
    public bool? FailOnExist { get; set; }

    [CliOption("--metadata")]
    public string? Metadata { get; set; }

    [CliFlag("--prevent-encryption-scope-override")]
    public bool? PreventEncryptionScopeOverride { get; set; }

    [CliOption("--public-access")]
    public string? PublicAccess { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}