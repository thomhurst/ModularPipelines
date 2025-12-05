using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privateca", "templates", "replicate")]
public record GcloudPrivatecaTemplatesReplicateOptions(
[property: CliArgument] string CertificateTemplate,
[property: CliArgument] string Location,
[property: CliFlag("--all-locations")] bool AllLocations,
[property: CliOption("--target-locations")] string[] TargetLocations
) : GcloudOptions
{
    [CliFlag("--continue-on-error")]
    public bool? ContinueOnError { get; set; }

    [CliFlag("--overwrite")]
    public bool? Overwrite { get; set; }
}