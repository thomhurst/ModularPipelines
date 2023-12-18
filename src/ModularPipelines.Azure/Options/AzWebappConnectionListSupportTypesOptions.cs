using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "connection", "list-support-types")]
public record AzWebappConnectionListSupportTypesOptions : AzOptions
{
    [CommandSwitch("--target-type")]
    public string? TargetType { get; set; }
}