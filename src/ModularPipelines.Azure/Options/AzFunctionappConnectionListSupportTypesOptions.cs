using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "connection", "list-support-types")]
public record AzFunctionappConnectionListSupportTypesOptions : AzOptions
{
    [CommandSwitch("--target-type")]
    public string? TargetType { get; set; }
}