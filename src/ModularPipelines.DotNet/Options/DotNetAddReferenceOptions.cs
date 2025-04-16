using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetAddReferenceOptions : DotNetOptions
{
    public DotNetAddReferenceOptions(
        string project,
        string projectReferences
    )
    {
        CommandParts = ["add", "[<PROJECT>]", "reference"];

        Project = project;

        ProjectReferences = projectReferences;
    }

    public DotNetAddReferenceOptions(
        string projectReferences
    )
    {
        CommandParts = ["add", "[<PROJECT>]", "reference"];

        ProjectReferences = projectReferences;
    }

    [PositionalArgument(PlaceholderName = "[<PROJECT>]")]
    public string? Project { get; set; }

    [CommandSwitch("--framework")]
    public virtual string? Framework { get; set; }

    [BooleanCommandSwitch("--interactive")]
    public virtual bool? Interactive { get; set; }

    [PositionalArgument(PlaceholderName = "<PROJECT_REFERENCES>")]
    public string? ProjectReferences { get; set; }
}
