using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "connection", "list-support-types")]
public record AzContainerappConnectionListSupportTypesOptions : AzOptions
{
    [CommandSwitch("--target-type")]
    public string? TargetType { get; set; }
}

