using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "api-portal", "update")]
public record AzSpringApiPortalUpdateOptions(
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

    [BooleanCommandSwitch("--enable-api-try-out")]
    public bool? EnableApiTryOut { get; set; }

    [BooleanCommandSwitch("--https-only")]
    public bool? HttpsOnly { get; set; }

    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--issuer-uri")]
    public string? IssuerUri { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}