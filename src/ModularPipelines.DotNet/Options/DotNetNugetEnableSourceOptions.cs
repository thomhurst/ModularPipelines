using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetNugetEnableSourceOptions : DotNetOptions
{
    public DotNetNugetEnableSourceOptions(
        string name
    )
    {
        CommandParts = ["nuget", "enable", "source", "<NAME>"];

        Name = name;
    }

    [PositionalArgument(PlaceholderName = "<NAME>")]
    public string? Name { get; set; }

    [CommandSwitch("--configfile")]
    public string? Configfile { get; set; }
}
