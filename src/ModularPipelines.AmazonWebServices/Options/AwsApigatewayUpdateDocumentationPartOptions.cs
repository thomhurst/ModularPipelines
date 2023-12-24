using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "update-documentation-part")]
public record AwsApigatewayUpdateDocumentationPartOptions(
[property: CommandSwitch("--rest-api-id")] string RestApiId,
[property: CommandSwitch("--documentation-part-id")] string DocumentationPartId
) : AwsOptions
{
    [CommandSwitch("--patch-operations")]
    public string[]? PatchOperations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}