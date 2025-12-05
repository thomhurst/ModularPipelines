using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "project-info", "remove-metadata")]
public record GcloudComputeProjectInfoRemoveMetadataOptions : GcloudOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }

    [CliOption("--keys")]
    public string[]? Keys { get; set; }
}