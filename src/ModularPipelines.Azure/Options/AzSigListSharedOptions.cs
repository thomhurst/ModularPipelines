using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sig", "list-shared")]
public record AzSigListSharedOptions(
[property: CliOption("--location")] string Location
) : AzOptions
{
    [CliOption("--shared-to")]
    public string? SharedTo { get; set; }
}