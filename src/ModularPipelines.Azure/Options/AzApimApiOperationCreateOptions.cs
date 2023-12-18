using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apim", "api", "operation", "create")]
public record AzApimApiOperationCreateOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--method")] string Method,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--service-name")] string ServiceName,
[property: CommandSwitch("--url-template")] string UrlTemplate
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--operation-id")]
    public string? OperationId { get; set; }

    [CommandSwitch("--params")]
    public string? Params { get; set; }
}

