using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "sign-url")]
public record GcloudComputeSignUrlOptions(
[property: PositionalArgument] string Url,
[property: CommandSwitch("--expires-in")] string ExpiresIn,
[property: CommandSwitch("--key-file")] string KeyFile,
[property: CommandSwitch("--key-name")] string KeyName
) : GcloudOptions
{
    [BooleanCommandSwitch("--validate")]
    public bool? Validate { get; set; }
}