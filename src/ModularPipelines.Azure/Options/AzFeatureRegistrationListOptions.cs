using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("feature", "registration", "list")]
public record AzFeatureRegistrationListOptions : AzOptions
{
    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }
}