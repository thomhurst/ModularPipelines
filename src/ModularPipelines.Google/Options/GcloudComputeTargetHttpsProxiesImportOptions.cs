using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "target-https-proxies", "import")]
public record GcloudComputeTargetHttpsProxiesImportOptions(
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}