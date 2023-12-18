using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "api", "create")]
public record AzApimApiCreateOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
    [CommandSwitch("--api-type")]
    public string? ApiType { get; set; }

    [CommandSwitch("--authorization-scope")]
    public string? AuthorizationScope { get; set; }

    [CommandSwitch("--authorization-server-id")]
    public string? AuthorizationServerId { get; set; }

    [CommandSwitch("--bearer-token-sending-methods")]
    public string? BearerTokenSendingMethods { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--open-id-provider-id")]
    public string? OpenIdProviderId { get; set; }

    [CommandSwitch("--protocols")]
    public string? Protocols { get; set; }

    [CommandSwitch("--service-url")]
    public string? ServiceUrl { get; set; }

    [CommandSwitch("--subscription-key-header-name")]
    public string? SubscriptionKeyHeaderName { get; set; }

    [CommandSwitch("--subscription-key-query-param-name")]
    public string? SubscriptionKeyQueryParamName { get; set; }

    [CommandSwitch("--subscription-key-required")]
    public string? SubscriptionKeyRequired { get; set; }

    [BooleanCommandSwitch("--subscription-required")]
    public bool? SubscriptionRequired { get; set; }
}