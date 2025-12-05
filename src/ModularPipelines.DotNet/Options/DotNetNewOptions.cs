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

    [CliArgument(Name = "<TEMPLATE>")]
    public virtual string? Template { get; set; }

    [CliFlag("--dry-run")]
    public virtual bool? DryRun { get; set; }

    [CliFlag("--force")]
    public virtual bool? Force { get; set; }

    [CliFlag("--language")]
    public virtual bool? Language { get; set; }

    [CliOption("--name")]
    public virtual string? Name { get; set; }

    [CliOption("--framework")]
    public virtual string? Framework { get; set; }

    [CliFlag("--no-update-check")]
    public virtual bool? NoUpdateCheck { get; set; }

    [CliOption("--output")]
    public virtual string? OutputDirectory { get; set; }

    [CliOption("--project")]
    public virtual string? ProjectPath { get; set; }

    [CliFlag("--diagnostics")]
    public virtual bool? Diagnostics { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }
}
