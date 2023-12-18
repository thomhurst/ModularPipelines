using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "api", "show")]
public record AzApimApiShowOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--api-type")]
    public string? ApiType { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--protocols")]
    public string? Protocols { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--service-url")]
    public string? ServiceUrl { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription-key-header-name")]
    public string? SubscriptionKeyHeaderName { get; set; }

    [CommandSwitch("--subscription-key-query-param-name")]
    public string? SubscriptionKeyQueryParamName { get; set; }

    [BooleanCommandSwitch("--subscription-required")]
    public bool? SubscriptionRequired { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

