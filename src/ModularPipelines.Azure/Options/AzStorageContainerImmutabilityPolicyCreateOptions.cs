using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "container", "immutability-policy", "create")]
public record AzStorageContainerImmutabilityPolicyCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--container-name")] string ContainerName
) : AzOptions
{
    [BooleanCommandSwitch("--allow-protected-append-writes")]
    public bool? AllowProtectedAppendWrites { get; set; }

    [BooleanCommandSwitch("--allow-protected-append-writes-all")]
    public bool? AllowProtectedAppendWritesAll { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--period")]
    public string? Period { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}