using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "container", "immutability-policy", "create")]
public record AzStorageContainerImmutabilityPolicyCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--container-name")] string ContainerName
) : AzOptions
{
    [CliFlag("--allow-protected-append-writes")]
    public bool? AllowProtectedAppendWrites { get; set; }

    [CliFlag("--allow-protected-append-writes-all")]
    public bool? AllowProtectedAppendWritesAll { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--period")]
    public string? Period { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}