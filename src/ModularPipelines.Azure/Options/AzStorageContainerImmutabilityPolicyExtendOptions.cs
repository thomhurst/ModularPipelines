using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "container", "immutability-policy", "extend")]
public record AzStorageContainerImmutabilityPolicyExtendOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--if-match")] string IfMatch
) : AzOptions
{
    [BooleanCommandSwitch("--allow-protected-append-writes")]
    public bool? AllowProtectedAppendWrites { get; set; }

    [BooleanCommandSwitch("--allow-protected-append-writes-all")]
    public bool? AllowProtectedAppendWritesAll { get; set; }

    [CommandSwitch("--period")]
    public string? Period { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

