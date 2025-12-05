using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instance-groups", "managed", "set-instance-template")]
public record GcloudComputeInstanceGroupsManagedSetInstanceTemplateOptions(
[property: CliArgument] string Name,
[property: CliOption("--template")] string Template
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}