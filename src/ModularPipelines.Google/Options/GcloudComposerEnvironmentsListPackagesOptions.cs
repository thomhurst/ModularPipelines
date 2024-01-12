using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("composer", "environments", "list-packages")]
public record GcloudComposerEnvironmentsListPackagesOptions(
[property: PositionalArgument] string Environment,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [BooleanCommandSwitch("--tree")]
    public bool? Tree { get; set; }
}