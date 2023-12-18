using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "cache", "create")]
public record AzAcrCacheCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry,
[property: CommandSwitch("--source-repo")] string SourceRepo,
[property: CommandSwitch("--target-repo")] string TargetRepo
) : AzOptions
{
    [CommandSwitch("--cred-set")]
    public string? CredSet { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}