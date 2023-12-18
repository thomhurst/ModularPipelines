using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("provider", "register")]
public record AzProviderRegisterOptions(
[property: CommandSwitch("--namespace")] string Namespace
) : AzOptions
{
    [BooleanCommandSwitch("--consent-to-permissions")]
    public bool? ConsentToPermissions { get; set; }

    [CommandSwitch("--management-group-id")]
    public string? ManagementGroupId { get; set; }

    [BooleanCommandSwitch("--wait")]
    public bool? Wait { get; set; }
}