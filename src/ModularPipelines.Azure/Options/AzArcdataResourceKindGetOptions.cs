using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcdata", "resource-kind", "get")]
public record AzArcdataResourceKindGetOptions(
[property: CliOption("--kind")] string Kind
) : AzOptions
{
    [CliOption("--dest")]
    public string? Dest { get; set; }
}