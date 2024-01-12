using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scc", "custom-modules", "sha", "list-descendant")]
public record GcloudSccCustomModulesShaListDescendantOptions : GcloudOptions
{
    public GcloudSccCustomModulesShaListDescendantOptions(
        string folder,
        string organization,
        string project
    )
    {
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }
}
