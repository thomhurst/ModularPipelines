using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetNugetRemoveSourceOptions : DotNetOptions
{
    public DotNetNugetRemoveSourceOptions(
        string name
    )
    {
        CommandParts = ["nuget", "remove", "source", "<NAME>"];

        Name = name;
    }

    [CliArgument(Name = "<NAME>")]
    public string? Name { get; set; }

    [CliOption("--configfile")]
    public virtual string? Configfile { get; set; }
}
