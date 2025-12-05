using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("lock", "create")]
public record AzLockCreateOptions(
[property: CliOption("--lock-type")] string LockType,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--notes")]
    public string? Notes { get; set; }

    [CliOption("--parent")]
    public string? Parent { get; set; }

    [CliOption("--resource")]
    public string? Resource { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }
}