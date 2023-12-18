using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud", "connection", "list-support-types")]
public record AzSpringCloudConnectionListSupportTypesOptions : AzOptions
{
    [CommandSwitch("--target-type")]
    public string? TargetType { get; set; }
}

