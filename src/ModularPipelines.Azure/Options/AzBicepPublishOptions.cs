using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bicep", "publish")]
public record AzBicepPublishOptions(
[property: CliOption("--file")] string File,
[property: CliOption("--target")] string Target
) : AzOptions
{
    [CliOption("--documentationUri")]
    public string? DocumentationUri { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }
}