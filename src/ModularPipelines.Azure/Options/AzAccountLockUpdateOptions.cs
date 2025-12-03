using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "lock", "update")]
public record AzAccountLockUpdateOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--lock-type")]
    public string? LockType { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--notes")]
    public string? Notes { get; set; }
}