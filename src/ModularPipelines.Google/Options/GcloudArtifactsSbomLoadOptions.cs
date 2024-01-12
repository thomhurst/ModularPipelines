using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "sbom", "load")]
public record GcloudArtifactsSbomLoadOptions(
[property: CommandSwitch("--source")] string Source,
[property: CommandSwitch("--uri")] string Uri
) : GcloudOptions
{
    [CommandSwitch("--destination")]
    public string? Destination { get; set; }

    [CommandSwitch("--kms-key-version")]
    public string? KmsKeyVersion { get; set; }
}