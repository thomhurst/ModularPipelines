using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("scc", "custom-modules", "sha", "simulate")]
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

    [CliOption("--custom-config-from-file")]
    public string CustomConfigFromFile { get; set; }

    [CliOption("--resource-from-file")]
    public string ResourceFromFile { get; set; }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }
}
