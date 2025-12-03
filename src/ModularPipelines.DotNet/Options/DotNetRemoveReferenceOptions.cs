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

    [CliArgument(Name = "[<PROJECT>]")]
    public string? Project { get; set; }

    [CliOption("--framework")]
    public virtual string? Framework { get; set; }

    [CliArgument(Name = "<PROJECT_REFERENCES>")]
    public string? ProjectReferences { get; set; }
}
