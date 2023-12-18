using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connection", "list-support-types")]
public record AzConnectionListSupportTypesOptions : AzOptions
{
    [CommandSwitch("--target-type")]
    public string? TargetType { get; set; }
}