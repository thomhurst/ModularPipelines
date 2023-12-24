using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "get-documentation-parts")]
public record AwsApigatewayGetDocumentationPartsOptions(
[property: CommandSwitch("--rest-api-id")] string RestApiId
) : AwsOptions
{
    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--name-query")]
    public string? NameQuery { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--location-status")]
    public string? LocationStatus { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}