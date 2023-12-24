using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "create-rest-api")]
public record AwsApigatewayCreateRestApiOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--clone-from")]
    public string? CloneFrom { get; set; }

    [CommandSwitch("--binary-media-types")]
    public string[]? BinaryMediaTypes { get; set; }

    [CommandSwitch("--minimum-compression-size")]
    public int? MinimumCompressionSize { get; set; }

    [CommandSwitch("--api-key-source")]
    public string? ApiKeySource { get; set; }

    [CommandSwitch("--endpoint-configuration")]
    public string? EndpointConfiguration { get; set; }

    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}