using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("publicca", "external-account-keys", "create")]
public record GcloudPubliccaExternalAccountKeysCreateOptions : GcloudOptions
{
    [CommandSwitch("--key-output-file")]
    public string? KeyOutputFile { get; set; }
}