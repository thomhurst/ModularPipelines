using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetNugetDisableSourceOptions : DotNetOptions
{
    public DotNetNugetDisableSourceOptions(
        string name
    )
    {
        CommandParts = ["nuget", "disable", "source", "<NAME>"];

        Name = name;
    }

    [PositionalArgument(PlaceholderName = "<NAME>")]
    public string? Name { get; set; }

    [CommandSwitch("--configfile")]
    public virtual string? Configfile { get; set; }
}
