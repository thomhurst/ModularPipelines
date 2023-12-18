using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("edgeorder", "list-metadata")]
public record AzEdgeorderListMetadataOptions : AzOptions
{
    [CommandSwitch("--skip-token")]
    public string? SkipToken { get; set; }
}