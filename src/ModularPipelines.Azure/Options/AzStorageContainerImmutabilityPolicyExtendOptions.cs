using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "container", "immutability-policy", "extend")]
public record AzStorageContainerImmutabilityPolicyExtendOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--container-name")] string ContainerName,
[property: CliOption("--if-match")] string IfMatch
) : AzOptions
{
    [CliFlag("--allow-protected-append-writes")]
    public bool? AllowProtectedAppendWrites { get; set; }

    [CliFlag("--allow-protected-append-writes-all")]
    public bool? AllowProtectedAppendWritesAll { get; set; }

    [CliOption("--period")]
    public string? Period { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}