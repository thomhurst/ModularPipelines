using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secrets", "delete")]
public record GcloudSecretsDeleteOptions(
[property: PositionalArgument] string Secret
) : GcloudOptions
{
    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}