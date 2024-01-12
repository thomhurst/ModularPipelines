using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("privateca", "templates", "replicate")]
public record GcloudPrivatecaTemplatesReplicateOptions(
[property: PositionalArgument] string CertificateTemplate,
[property: PositionalArgument] string Location,
[property: BooleanCommandSwitch("--all-locations")] bool AllLocations,
[property: CommandSwitch("--target-locations")] string[] TargetLocations
) : GcloudOptions
{
    [BooleanCommandSwitch("--continue-on-error")]
    public bool? ContinueOnError { get; set; }

    [BooleanCommandSwitch("--overwrite")]
    public bool? Overwrite { get; set; }
}