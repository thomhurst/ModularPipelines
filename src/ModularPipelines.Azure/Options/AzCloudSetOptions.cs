using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cloud", "set")]
public record AzCloudSetOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--profile")]
    public string? Profile { get; set; }
}