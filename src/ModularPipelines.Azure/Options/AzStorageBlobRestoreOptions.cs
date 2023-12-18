using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "blob", "restore")]
public record AzStorageBlobRestoreOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--time-to-restore")] string TimeToRestore
) : AzOptions
{
    [CommandSwitch("--blob-range")]
    public string? BlobRange { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}