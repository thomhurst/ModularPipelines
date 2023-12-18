using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mesh", "code-package-log", "get")]
public record AzMeshCodePackageLogGetOptions : AzOptions
{
    [CommandSwitch("--app-name")]
    public string? AppName { get; set; }

    [CommandSwitch("--code-package-name")]
    public string? CodePackageName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--replica-name")]
    public string? ReplicaName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--service-name")]
    public string? ServiceName { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tail")]
    public string? Tail { get; set; }
}