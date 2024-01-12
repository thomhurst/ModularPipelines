using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("api-gateway", "api-configs", "list")]
public record GcloudApiGatewayApiConfigsListOptions : GcloudOptions
{
    [CommandSwitch("--api")]
    public string? Api { get; set; }
}