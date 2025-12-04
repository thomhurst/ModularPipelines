using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcappliance", "get-credentials")]
public record AzArcapplianceGetCredentialsOptions : AzOptions
{
    [CliOption("--config-file")]
    public string? ConfigFile { get; set; }

    [CliOption("--credentials-dir")]
    public string? CredentialsDir { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--overwrite-existing")]
    public bool? OverwriteExisting { get; set; }

    [CliFlag("--partner")]
    public bool? Partner { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}