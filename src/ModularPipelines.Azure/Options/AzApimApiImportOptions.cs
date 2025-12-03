using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apim", "api", "import")]
public record AzApimApiImportOptions(
[property: CliOption("--path")] string Path,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--specification-format")] string SpecificationFormat
) : AzOptions
{
    [CliOption("--api-id")]
    public string? ApiId { get; set; }

    [CliOption("--api-revision")]
    public string? ApiRevision { get; set; }

    [CliOption("--api-type")]
    public string? ApiType { get; set; }

    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--api-version-set-id")]
    public string? ApiVersionSetId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--protocols")]
    public string? Protocols { get; set; }

    [CliOption("--service-url")]
    public string? ServiceUrl { get; set; }

    [CliOption("--soap-api-type")]
    public string? SoapApiType { get; set; }

    [CliOption("--specification-path")]
    public string? SpecificationPath { get; set; }

    [CliOption("--specification-url")]
    public string? SpecificationUrl { get; set; }

    [CliOption("--subscription-key-header-name")]
    public string? SubscriptionKeyHeaderName { get; set; }

    [CliOption("--subscription-key-query-param-name")]
    public string? SubscriptionKeyQueryParamName { get; set; }

    [CliFlag("--subscription-required")]
    public bool? SubscriptionRequired { get; set; }

    [CliOption("--wsdl-endpoint-name")]
    public string? WsdlEndpointName { get; set; }

    [CliOption("--wsdl-service-name")]
    public string? WsdlServiceName { get; set; }
}