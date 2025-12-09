using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hdinsight", "script-action", "execute")]
public record AzHdinsightScriptActionExecuteOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--roles")] string Roles,
[property: CliOption("--script-uri")] string ScriptUri
) : AzOptions
{
    [CliFlag("--persist-on-success")]
    public bool? PersistOnSuccess { get; set; }

    [CliOption("--script-parameters")]
    public string? ScriptParameters { get; set; }
}