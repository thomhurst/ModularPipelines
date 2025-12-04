using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mesh", "code-package-log", "get")]
public record AzMeshCodePackageLogGetOptions : AzOptions
{
    [CliOption("--app-name")]
    public string? AppName { get; set; }

    [CliOption("--code-package-name")]
    public string? CodePackageName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--replica-name")]
    public string? ReplicaName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service-name")]
    public string? ServiceName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tail")]
    public string? Tail { get; set; }
}