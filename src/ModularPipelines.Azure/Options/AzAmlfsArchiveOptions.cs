using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amlfs", "archive")]
public record AzAmlfsArchiveOptions : AzOptions
{
    [CommandSwitch("--amlfs-name")]
    public string? AmlfsName { get; set; }

    [CommandSwitch("--filesystem-path")]
    public string? FilesystemPath { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}