using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("publicca", "external-account-keys", "create")]
public record GcloudPubliccaExternalAccountKeysCreateOptions : GcloudOptions
{
    [CliOption("--key-output-file")]
    public string? KeyOutputFile { get; set; }
}