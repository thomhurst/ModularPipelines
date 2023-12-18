using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "container", "legal-hold", "clear")]
public record AzStorageContainerLegalHoldClearOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--tags")] string Tags
) : AzOptions
{
    [BooleanCommandSwitch("--allow-protected-append-writes-all")]
    public bool? AllowProtectedAppendWritesAll { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}