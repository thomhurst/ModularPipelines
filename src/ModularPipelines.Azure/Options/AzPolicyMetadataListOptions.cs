using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("policy", "metadata", "list")]
public record AzPolicyMetadataListOptions : AzOptions
{
    [CommandSwitch("--top")]
    public string? Top { get; set; }
}