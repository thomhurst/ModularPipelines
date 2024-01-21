using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetNewOptions : DotNetOptions
{
    [PositionalArgument(PlaceholderName = "<TEMPLATE>")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--language")]
    public bool? Language { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--framework")]
    public string? Framework { get; set; }

    [BooleanCommandSwitch("--no-update-check")]
    public bool? NoUpdateCheck { get; set; }

    [CommandSwitch("--output")]
    public string? OutputDirectory { get; set; }

    [CommandSwitch("--project")]
    public string? ProjectPath { get; set; }

    [BooleanCommandSwitch("--diagnostics")]
    public bool? Diagnostics { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }
}
