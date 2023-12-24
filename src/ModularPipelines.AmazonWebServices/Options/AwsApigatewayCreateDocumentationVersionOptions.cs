using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "create-documentation-version")]
public record AwsApigatewayCreateDocumentationVersionOptions(
[property: CommandSwitch("--rest-api-id")] string RestApiId,
[property: CommandSwitch("--documentation-version")] string DocumentationVersion
) : AwsOptions
{
    [CommandSwitch("--stage-name")]
    public string? StageName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}