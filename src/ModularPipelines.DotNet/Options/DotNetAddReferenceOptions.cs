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

    [CliArgument(Name = "[<PROJECT>]")]
    public string? Project { get; set; }

    [CliOption("--framework")]
    public virtual string? Framework { get; set; }

    [CliFlag("--interactive")]
    public virtual bool? Interactive { get; set; }

    [CliArgument(Name = "<PROJECT_REFERENCES>")]
    public string? ProjectReferences { get; set; }
}
