using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("batch", "file", "upload")]
public record AzBatchFileUploadOptions(
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

    [CommandSwitch("--flatten")]
    public string? Flatten { get; set; }

    [CommandSwitch("--remote-path")]
    public string? RemotePath { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}