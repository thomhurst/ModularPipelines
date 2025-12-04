using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "share", "url")]
public record AzStorageShareUrlOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--file-endpoint")]
    public string? FileEndpoint { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliFlag("--unc")]
    public bool? Unc { get; set; }
}