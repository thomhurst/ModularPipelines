using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "blob", "service-properties", "delete-policy", "update")]
public record AzStorageBlobServicePropertiesDeletePolicyUpdateOptions : AzOptions
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

    [CliOption("--days-retained")]
    public string? DaysRetained { get; set; }

    [CliFlag("--enable")]
    public bool? Enable { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }
}