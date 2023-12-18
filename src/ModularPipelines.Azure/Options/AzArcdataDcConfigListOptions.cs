using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "dc", "config", "list")]
public record AzArcdataDcConfigListOptions : AzOptions
{
    [CommandSwitch("--config-profile")]
    public string? ConfigProfile { get; set; }
}