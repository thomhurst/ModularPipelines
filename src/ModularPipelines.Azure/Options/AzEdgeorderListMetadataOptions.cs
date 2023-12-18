using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edgeorder", "list-metadata")]
public record AzEdgeorderListMetadataOptions : AzOptions
{
    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }
}

