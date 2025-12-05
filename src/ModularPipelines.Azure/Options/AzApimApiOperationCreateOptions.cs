using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("apim", "api", "operation", "create")]
public record AzApimApiOperationCreateOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--method")] string Method,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName,
[property: CliOption("--url-template")] string UrlTemplate
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--operation-id")]
    public string? OperationId { get; set; }

    [CliOption("--params")]
    public string? Params { get; set; }
}