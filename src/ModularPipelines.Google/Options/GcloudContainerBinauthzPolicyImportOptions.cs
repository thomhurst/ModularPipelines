using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "binauthz", "policy", "import")]
public record GcloudContainerBinauthzPolicyImportOptions(
[property: CliArgument] string PolicyFile
) : GcloudOptions
{
    [CliFlag("--strict-validation")]
    public bool? StrictValidation { get; set; }
}