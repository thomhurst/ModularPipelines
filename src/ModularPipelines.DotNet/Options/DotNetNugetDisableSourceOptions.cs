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

    [CliArgument(Name = "<NAME>")]
    public virtual string? Name { get; set; }

    [CliOption("--configfile")]
    public virtual string? Configfile { get; set; }
}
