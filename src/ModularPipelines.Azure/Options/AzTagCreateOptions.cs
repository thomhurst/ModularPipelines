using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tag", "create")]
public record AzTagCreateOptions : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-id")]
    public string? ResourceId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}