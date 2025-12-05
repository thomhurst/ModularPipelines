using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "get-credentials")]
public record AzAksGetCredentialsOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--admin")]
    public bool? Admin { get; set; }

    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--file")]
    public string? File { get; set; }

    [CliOption("--format")]
    public string? Format { get; set; }

    [CliFlag("--overwrite-existing")]
    public bool? OverwriteExisting { get; set; }

    [CliFlag("--public-fqdn")]
    public bool? PublicFqdn { get; set; }
}