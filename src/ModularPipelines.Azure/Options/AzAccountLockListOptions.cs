using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("account", "lock", "list")]
public record AzAccountLockListOptions : AzOptions
{
    [CliOption("--filter-string")]
    public string? FilterString { get; set; }
}