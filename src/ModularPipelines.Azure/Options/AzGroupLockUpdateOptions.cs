using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("group", "lock", "update")]
public record AzGroupLockUpdateOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--lock-type")]
    public string? LockType { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--notes")]
    public string? Notes { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}