using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "api", "import")]
public record AzApimApiImportOptions(
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--specification-format")] string SpecificationFormat
) : AzOptions
{
    [CommandSwitch("--api-id")]
    public string? ApiId { get; set; }

    [CommandSwitch("--api-revision")]
    public string? ApiRevision { get; set; }

    [CommandSwitch("--api-type")]
    public string? ApiType { get; set; }

    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--api-version-set-id")]
    public string? ApiVersionSetId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--protocols")]
    public string? Protocols { get; set; }

    [CommandSwitch("--service-url")]
    public string? ServiceUrl { get; set; }

    [CommandSwitch("--soap-api-type")]
    public string? SoapApiType { get; set; }

    [CommandSwitch("--specification-path")]
    public string? SpecificationPath { get; set; }

    [CommandSwitch("--specification-url")]
    public string? SpecificationUrl { get; set; }

    [CommandSwitch("--subscription-key-header-name")]
    public string? SubscriptionKeyHeaderName { get; set; }

    [CommandSwitch("--subscription-key-query-param-name")]
    public string? SubscriptionKeyQueryParamName { get; set; }

    [BooleanCommandSwitch("--subscription-required")]
    public bool? SubscriptionRequired { get; set; }

    [CommandSwitch("--wsdl-endpoint-name")]
    public string? WsdlEndpointName { get; set; }

    [CommandSwitch("--wsdl-service-name")]
    public string? WsdlServiceName { get; set; }
}