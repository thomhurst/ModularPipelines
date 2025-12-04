using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim", "api", "create")]
public record AzApimApiCreateOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--path")] string Path,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliOption("--api-type")]
    public string? ApiType { get; set; }

    [CliOption("--authorization-scope")]
    public string? AuthorizationScope { get; set; }

    [CliOption("--authorization-server-id")]
    public string? AuthorizationServerId { get; set; }

    [CliOption("--bearer-token-sending-methods")]
    public string? BearerTokenSendingMethods { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--open-id-provider-id")]
    public string? OpenIdProviderId { get; set; }

    [CliOption("--protocols")]
    public string? Protocols { get; set; }

    [CliOption("--service-url")]
    public string? ServiceUrl { get; set; }

    [CliOption("--subscription-key-header-name")]
    public string? SubscriptionKeyHeaderName { get; set; }

    [CliOption("--subscription-key-query-param-name")]
    public string? SubscriptionKeyQueryParamName { get; set; }

    [CliOption("--subscription-key-required")]
    public string? SubscriptionKeyRequired { get; set; }

    [CliFlag("--subscription-required")]
    public bool? SubscriptionRequired { get; set; }
}