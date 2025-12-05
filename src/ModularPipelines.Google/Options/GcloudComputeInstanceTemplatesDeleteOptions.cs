using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-templates", "delete")]
public record GcloudComputeInstanceTemplatesDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}