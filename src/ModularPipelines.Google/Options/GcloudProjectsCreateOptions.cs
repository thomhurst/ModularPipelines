using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("projects", "create")]
public record GcloudProjectsCreateOptions(
[property: CliArgument] string ProjectId
) : GcloudOptions
{
    [CliFlag("--enable-cloud-apis")]
    public bool? EnableCloudApis { get; set; }

    [CliOption("--folder")]
    public string? Folder { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--organization")]
    public string? Organization { get; set; }

    [CliFlag("--set-as-default")]
    public bool? SetAsDefault { get; set; }
}