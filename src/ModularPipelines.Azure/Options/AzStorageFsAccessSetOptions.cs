using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "fs", "access", "set")]
public record AzStorageFsAccessSetOptions(
[property: CliOption("--file-system")] string FileSystem,
[property: CliOption("--path")] string Path
) : AzOptions
{
    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--acl")]
    public string? Acl { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--blob-endpoint")]
    public string? BlobEndpoint { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--group")]
    public string? Group { get; set; }

    [CliOption("--owner")]
    public string? Owner { get; set; }

    [CliOption("--permissions")]
    public string? Permissions { get; set; }

    [CliOption("--sas-token")]
    public string? SasToken { get; set; }
}