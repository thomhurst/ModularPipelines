using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "project-info", "add-metadata")]
public record GcloudComputeProjectInfoAddMetadataOptions : GcloudOptions
{
    [CommandSwitch("--metadata")]
    public IEnumerable<KeyValue>? Metadata { get; set; }

    [CommandSwitch("--metadata-from-file")]
    public string[]? MetadataFromFile { get; set; }
}