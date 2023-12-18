using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("extension", "list-available")]
public record AzExtensionListAvailableOptions : AzOptions
{
    [BooleanCommandSwitch("--show-details")]
    public bool? ShowDetails { get; set; }
}