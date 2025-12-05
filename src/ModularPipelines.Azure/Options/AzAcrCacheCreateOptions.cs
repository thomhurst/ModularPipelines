using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "cache", "create")]
public record AzAcrCacheCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry,
[property: CliOption("--source-repo")] string SourceRepo,
[property: CliOption("--target-repo")] string TargetRepo
) : AzOptions
{
    [CliOption("--cred-set")]
    public string? CredSet { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}