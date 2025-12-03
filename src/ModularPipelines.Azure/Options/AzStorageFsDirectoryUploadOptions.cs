using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "fs", "directory", "upload")]
public record AzStorageFsDirectoryUploadOptions(
[property: CliOption("--file-system")] string FileSystem,
[property: CliOption("--source")] string Source
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--destination-path")]
    public string? DestinationPath { get; set; }

    [CliOption("--recursive")]
    public string? Recursive { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }
}