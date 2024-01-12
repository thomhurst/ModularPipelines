using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("scc", "custom-modules", "sha", "simulate")]
public record GcloudSccCustomModulesShaSimulateOptions : GcloudOptions
{
    public GcloudSccCustomModulesShaSimulateOptions(
        string customConfigFromFile,
        string resourceFromFile,
        string folder,
        string organization,
        string project
    )
    {
        CustomConfigFromFile = customConfigFromFile;
        ResourceFromFile = resourceFromFile;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [CommandSwitch("--custom-config-from-file")]
    public string CustomConfigFromFile { get; set; }

    [CommandSwitch("--resource-from-file")]
    public string ResourceFromFile { get; set; }

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }
}
