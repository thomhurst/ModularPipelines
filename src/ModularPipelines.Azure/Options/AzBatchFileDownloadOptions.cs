using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "file", "download")]
public record AzBatchFileDownloadOptions(
[property: CommandSwitch("--file-group")] string FileGroup,
[property: CommandSwitch("--local-path")] string LocalPath
) : AzOptions
{
    [CommandSwitch("--account-endpoint")]
    public int? AccountEndpoint { get; set; }

    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [BooleanCommandSwitch("--overwrite")]
    public bool? Overwrite { get; set; }

    [CommandSwitch("--remote-path")]
    public string? RemotePath { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}