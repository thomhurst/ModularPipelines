using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "remove")]
public record AzStorageRemoveOptions : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--container-name")]
    public string? ContainerName { get; set; }

    [CliOption("--exclude-path")]
    public string? ExcludePath { get; set; }

    [CliOption("--exclude-pattern")]
    public string? ExcludePattern { get; set; }

    [CliOption("--include-path")]
    public string? IncludePath { get; set; }

    [CliOption("--include-pattern")]
    public string? IncludePattern { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--recursive")]
    public string? Recursive { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--share-name")]
    public string? ShareName { get; set; }
}