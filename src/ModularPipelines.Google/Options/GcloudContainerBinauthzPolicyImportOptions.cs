using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("container", "binauthz", "policy", "import")]
public record GcloudContainerBinauthzPolicyImportOptions(
[property: PositionalArgument] string PolicyFile
) : GcloudOptions
{
    [BooleanCommandSwitch("--strict-validation")]
    public bool? StrictValidation { get; set; }
}