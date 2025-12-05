using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("composer", "environments", "list-packages")]
public record GcloudComposerEnvironmentsListPackagesOptions(
[property: CliArgument] string Environment,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--tree")]
    public bool? Tree { get; set; }
}