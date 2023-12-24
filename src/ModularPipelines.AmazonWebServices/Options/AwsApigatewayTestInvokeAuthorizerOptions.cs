using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "test-invoke-authorizer")]
public record AwsApigatewayTestInvokeAuthorizerOptions(
[property: CommandSwitch("--rest-api-id")] string RestApiId,
[property: CommandSwitch("--authorizer-id")] string AuthorizerId
) : AwsOptions
{
    [CommandSwitch("--headers")]
    public IEnumerable<KeyValue>? Headers { get; set; }

    [CommandSwitch("--multi-value-headers")]
    public IEnumerable<KeyValue>? MultiValueHeaders { get; set; }

    [CommandSwitch("--path-with-query-string")]
    public string? PathWithQueryString { get; set; }

    [CommandSwitch("--body")]
    public string? Body { get; set; }

    [CommandSwitch("--stage-variables")]
    public IEnumerable<KeyValue>? StageVariables { get; set; }

    [CommandSwitch("--additional-context")]
    public IEnumerable<KeyValue>? AdditionalContext { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}