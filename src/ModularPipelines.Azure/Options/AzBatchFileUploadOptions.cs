using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("batch", "file", "upload")]
public record AzBatchFileUploadOptions(
[property: CliOption("--file-group")] string FileGroup,
[property: CliOption("--local-path")] string LocalPath
) : AzOptions
{
    [CliOption("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CliOption("--account-key")]
    public int? AccountKey { get; set; }

    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--flatten")]
    public string? Flatten { get; set; }

    [CliOption("--remote-path")]
    public string? RemotePath { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}