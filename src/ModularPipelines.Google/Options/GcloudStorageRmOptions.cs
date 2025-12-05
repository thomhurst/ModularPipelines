using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "rm")]
public record GcloudStorageRmOptions(
[property: CliArgument] string Urls
) : GcloudOptions
{
    [CliOption("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CliFlag("--all-versions")]
    public bool? AllVersions { get; set; }

    [CliFlag("--continue-on-error")]
    public bool? ContinueOnError { get; set; }

    [CliFlag("--read-paths-from-stdin")]
    public bool? ReadPathsFromStdin { get; set; }

    [CliFlag("--recursive")]
    public bool? Recursive { get; set; }
}