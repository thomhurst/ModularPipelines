using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("group", "lock", "create")]
public record AzGroupLockCreateOptions(
[property: CliOption("--lock-type")] string LockType,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--notes")]
    public string? Notes { get; set; }
}