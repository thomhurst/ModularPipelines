using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "url-maps", "import")]
public record GcloudComputeUrlMapsImportOptions(
[property: PositionalArgument] string UrlMap
) : GcloudOptions
{
    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}