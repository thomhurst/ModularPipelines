using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "lock", "create")]
public record AzAccountLockCreateOptions(
[property: CliOption("--lock-type")] string LockType,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--notes")]
    public string? Notes { get; set; }
}