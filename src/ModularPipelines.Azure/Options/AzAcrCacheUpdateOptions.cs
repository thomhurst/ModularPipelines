using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "cache", "update")]
public record AzAcrCacheUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--cred-set")]
    public string? CredSet { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliFlag("--remove-cred-set")]
    public bool? RemoveCredSet { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }
}