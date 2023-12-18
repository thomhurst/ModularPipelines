using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("hdinsight", "script-action", "execute")]
public record AzHdinsightScriptActionExecuteOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--roles")] string Roles,
[property: CommandSwitch("--script-uri")] string ScriptUri
) : AzOptions
{
    [BooleanCommandSwitch("--persist-on-success")]
    public bool? PersistOnSuccess { get; set; }

    [CommandSwitch("--script-parameters")]
    public string? ScriptParameters { get; set; }
}