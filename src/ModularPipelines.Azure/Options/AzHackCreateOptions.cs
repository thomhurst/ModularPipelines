using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hack", "create")]
public record AzHackCreateOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--runtime")] string Runtime
) : AzOptions
{
    [CliOption("--ai")]
    public string? Ai { get; set; }

    [CliOption("--database")]
    public string? Database { get; set; }
}