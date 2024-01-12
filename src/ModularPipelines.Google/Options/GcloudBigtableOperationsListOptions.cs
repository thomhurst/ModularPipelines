using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "operations", "list")]
public record GcloudBigtableOperationsListOptions : GcloudOptions
{
    [CommandSwitch("--instance")]
    public string? Instance { get; set; }
}