using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "job", "import", "create")]
public record AzDtJobImportCreateOptions(
[property: CommandSwitch("--data-file")] string DataFile,
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--ibc")] string Ibc,
[property: CommandSwitch("--input-storage-account")] int InputStorageAccount
) : AzOptions
{
    [CommandSwitch("--job-id")]
    public string? JobId { get; set; }

    [CommandSwitch("--obc")]
    public string? Obc { get; set; }

    [CommandSwitch("--of")]
    public string? Of { get; set; }

    [CommandSwitch("--osa")]
    public string? Osa { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}