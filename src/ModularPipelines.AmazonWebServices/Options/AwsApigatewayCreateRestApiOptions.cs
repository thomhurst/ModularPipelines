using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "create-rest-api")]
public record AwsApigatewayCreateRestApiOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--clone-from")]
    public string? CloneFrom { get; set; }

    [CliOption("--binary-media-types")]
    public string[]? BinaryMediaTypes { get; set; }

    [CliOption("--minimum-compression-size")]
    public int? MinimumCompressionSize { get; set; }

    [CliOption("--api-key-source")]
    public string? ApiKeySource { get; set; }

    [CliOption("--endpoint-configuration")]
    public string? EndpointConfiguration { get; set; }

    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--api-version")]
    public string? ApiVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}