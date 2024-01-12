using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("projects", "create")]
public record GcloudProjectsCreateOptions(
[property: PositionalArgument] string ProjectId
) : GcloudOptions
{
    [BooleanCommandSwitch("--enable-cloud-apis")]
    public bool? EnableCloudApis { get; set; }

    [CommandSwitch("--folder")]
    public string? Folder { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [BooleanCommandSwitch("--set-as-default")]
    public bool? SetAsDefault { get; set; }
}