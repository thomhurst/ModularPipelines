using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetListReferenceOptions : DotNetOptions
{
    public DotNetListReferenceOptions(
        string project
    )
    {
        CommandParts = ["list", "[<PROJECT>]", "reference"];

        Project = project;
    }

    public DotNetListReferenceOptions()
    {
        CommandParts = ["list", "[<PROJECT>]", "reference"];
    }

    [CliArgument(Name = "[<PROJECT>]")]
    public virtual string? Project { get; set; }
}
