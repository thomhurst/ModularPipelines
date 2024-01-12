using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "ssl-certificates", "update")]
public record GcloudAppSslCertificatesUpdateOptions(
[property: PositionalArgument] string Id
) : GcloudOptions
{
    [CommandSwitch("--certificate")]
    public string? Certificate { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--private-key")]
    public string? PrivateKey { get; set; }
}