using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "dev-tool", "update")]
public record AzSpringDevToolUpdateOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service")] string Service
) : AzOptions
{
    [BooleanCommandSwitch("--assign-endpoint")]
    public bool? AssignEndpoint { get; set; }

    [CommandSwitch("--client-id")]
    public string? ClientId { get; set; }

    [CommandSwitch("--client-secret")]
    public string? ClientSecret { get; set; }

    [CommandSwitch("--metadata-url")]
    public string? MetadataUrl { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--scopes")]
    public string? Scopes { get; set; }
}