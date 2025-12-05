using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("extension", "list-available")]
public record AzExtensionListAvailableOptions : AzOptions
{
    [CliFlag("--show-details")]
    public bool? ShowDetails { get; set; }
}