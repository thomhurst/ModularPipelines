using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "copy")]
public record AzStorageCopyOptions : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--blob-type")]
    public string? BlobType { get; set; }

    [CliOption("--cap-mbps")]
    public string? CapMbps { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--destination-blob")]
    public string? DestinationBlob { get; set; }

    [CliOption("--destination-container")]
    public string? DestinationContainer { get; set; }

    [CliOption("--destination-file-path")]
    public string? DestinationFilePath { get; set; }

    [CliOption("--destination-share")]
    public string? DestinationShare { get; set; }

    [CliOption("--exclude-path")]
    public string? ExcludePath { get; set; }

    [CliOption("--exclude-pattern")]
    public string? ExcludePattern { get; set; }

    [CliOption("--follow-symlinks")]
    public string? FollowSymlinks { get; set; }

    [CliOption("--include-path")]
    public string? IncludePath { get; set; }

    [CliOption("--include-pattern")]
    public string? IncludePattern { get; set; }

    [CliFlag("--preserve-s2s-access-tier")]
    public bool? PreserveS2sAccessTier { get; set; }

    [CliOption("--put-md5")]
    public string? PutMd5 { get; set; }

    [CliOption("--recursive")]
    public string? Recursive { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--source-account-key")]
    public int? SourceAccountKey { get; set; }

    [CliOption("--source-account-name")]
    public int? SourceAccountName { get; set; }

    [CliOption("--source-blob")]
    public string? SourceBlob { get; set; }

    [CliOption("--source-connection-string")]
    public string? SourceConnectionString { get; set; }

    [CliOption("--source-container")]
    public string? SourceContainer { get; set; }

    [CliOption("--source-file-path")]
    public string? SourceFilePath { get; set; }

    [CliOption("--source-sas")]
    public string? SourceSas { get; set; }

    [CliOption("--source-share")]
    public string? SourceShare { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? EXTRAOPTIONS { get; set; }
}