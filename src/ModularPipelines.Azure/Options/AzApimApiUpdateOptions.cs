using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim", "api", "update")]
public record AzApimApiUpdateOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--api-type")]
    public string? ApiType { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--protocols")]
    public string? Protocols { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--service-url")]
    public string? ServiceUrl { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription-key-header-name")]
    public string? SubscriptionKeyHeaderName { get; set; }

    [CliOption("--subscription-key-query-param-name")]
    public string? SubscriptionKeyQueryParamName { get; set; }

    [CliFlag("--subscription-required")]
    public bool? SubscriptionRequired { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}