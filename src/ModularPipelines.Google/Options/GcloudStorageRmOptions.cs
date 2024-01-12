using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "rm")]
public record GcloudStorageRmOptions(
[property: PositionalArgument] string Urls
) : GcloudOptions
{
    [CommandSwitch("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [BooleanCommandSwitch("--all-versions")]
    public bool? AllVersions { get; set; }

    [BooleanCommandSwitch("--continue-on-error")]
    public bool? ContinueOnError { get; set; }

    [BooleanCommandSwitch("--read-paths-from-stdin")]
    public bool? ReadPathsFromStdin { get; set; }

    [BooleanCommandSwitch("--recursive")]
    public bool? Recursive { get; set; }
}