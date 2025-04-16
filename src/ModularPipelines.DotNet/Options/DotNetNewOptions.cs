using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetNewOptions : DotNetOptions
{
    public DotNetNewOptions(
        string template
    )
    {
        CommandParts = ["new", "<TEMPLATE>"];

        Template = template;
    }

    [PositionalArgument(PlaceholderName = "<TEMPLATE>")]
    public string? Template { get; set; }

    [BooleanCommandSwitch("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [BooleanCommandSwitch("--force")]
    public virtual bool? Force { get; set; }

    [BooleanCommandSwitch("--language")]
    public virtual bool? Language { get; set; }

    [CommandSwitch("--name")]
    public virtual string? Name { get; set; }

    [CommandSwitch("--framework")]
    public virtual string? Framework { get; set; }

    [BooleanCommandSwitch("--no-update-check")]
    public virtual bool? NoUpdateCheck { get; set; }

    [CommandSwitch("--output")]
    public virtual string? OutputDirectory { get; set; }

    [CommandSwitch("--project")]
    public virtual string? ProjectPath { get; set; }

    [BooleanCommandSwitch("--diagnostics")]
    public virtual bool? Diagnostics { get; set; }

    [CommandSwitch("--verbosity")]
    public virtual string? Verbosity { get; set; }
}
