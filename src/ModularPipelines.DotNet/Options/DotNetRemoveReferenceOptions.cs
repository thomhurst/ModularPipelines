using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetRemoveReferenceOptions : DotNetOptions
{
    public DotNetRemoveReferenceOptions(
        string project,
        string projectReferences
    )
    {
        CommandParts = ["remove", "[<PROJECT>]", "reference"];

        Project = project;

        ProjectReferences = projectReferences;
    }

    public DotNetRemoveReferenceOptions(
        string projectReferences
    )
    {
        CommandParts = ["remove", "[<PROJECT>]", "reference"];

        ProjectReferences = projectReferences;
    }

    [PositionalArgument(PlaceholderName = "[<PROJECT>]")]
    public string? Project { get; set; }

    [CommandSwitch("--framework")]
    public virtual string? Framework { get; set; }

    [PositionalArgument(PlaceholderName = "<PROJECT_REFERENCES>")]
    public string? ProjectReferences { get; set; }
}
