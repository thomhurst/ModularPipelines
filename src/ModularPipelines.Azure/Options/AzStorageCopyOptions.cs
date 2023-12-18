using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "copy")]
public record AzStorageCopyOptions : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--blob-type")]
    public string? BlobType { get; set; }

    [CommandSwitch("--cap-mbps")]
    public string? CapMbps { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [CommandSwitch("--destination-blob")]
    public string? DestinationBlob { get; set; }

    [CommandSwitch("--destination-container")]
    public string? DestinationContainer { get; set; }

    [CommandSwitch("--destination-file-path")]
    public string? DestinationFilePath { get; set; }

    [CommandSwitch("--destination-share")]
    public string? DestinationShare { get; set; }

    [CommandSwitch("--exclude-path")]
    public string? ExcludePath { get; set; }

    [CommandSwitch("--exclude-pattern")]
    public string? ExcludePattern { get; set; }

    [CommandSwitch("--follow-symlinks")]
    public string? FollowSymlinks { get; set; }

    [CommandSwitch("--include-path")]
    public string? IncludePath { get; set; }

    [CommandSwitch("--include-pattern")]
    public string? IncludePattern { get; set; }

    [BooleanCommandSwitch("--preserve-s2s-access-tier")]
    public bool? PreserveS2sAccessTier { get; set; }

    [CommandSwitch("--put-md5")]
    public string? PutMd5 { get; set; }

    [CommandSwitch("--recursive")]
    public string? Recursive { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--source-account-key")]
    public int? SourceAccountKey { get; set; }

    [CommandSwitch("--source-account-name")]
    public int? SourceAccountName { get; set; }

    [CommandSwitch("--source-blob")]
    public string? SourceBlob { get; set; }

    [CommandSwitch("--source-connection-string")]
    public string? SourceConnectionString { get; set; }

    [CommandSwitch("--source-container")]
    public string? SourceContainer { get; set; }

    [CommandSwitch("--source-file-path")]
    public string? SourceFilePath { get; set; }

    [CommandSwitch("--source-sas")]
    public string? SourceSas { get; set; }

    [CommandSwitch("--source-share")]
    public string? SourceShare { get; set; }

    [PositionalArgument(Position = Position.AfterSwitches)]
    public string? ExtraOptions { get; set; }
}