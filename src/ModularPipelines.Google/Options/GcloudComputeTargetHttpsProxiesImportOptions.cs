using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "target-https-proxies", "import")]
public record GcloudComputeTargetHttpsProxiesImportOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--source")]
    public string? Source { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}