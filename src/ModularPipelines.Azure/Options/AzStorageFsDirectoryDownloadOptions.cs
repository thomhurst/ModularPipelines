using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "fs", "directory", "download")]
public record AzStorageFsDirectoryDownloadOptions(
[property: CliOption("--destination-path")] string DestinationPath,
[property: CliOption("--file-system")] string FileSystem
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--recursive")]
    public string? Recursive { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--source-path")]
    public string? SourcePath { get; set; }
}