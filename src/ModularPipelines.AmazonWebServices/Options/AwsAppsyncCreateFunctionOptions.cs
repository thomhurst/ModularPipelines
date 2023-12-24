using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "create-function")]
public record AwsAppsyncCreateFunctionOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--data-source-name")] string DataSourceName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--request-mapping-template")]
    public string? RequestMappingTemplate { get; set; }

    [CommandSwitch("--response-mapping-template")]
    public string? ResponseMappingTemplate { get; set; }

    [CommandSwitch("--function-version")]
    public string? FunctionVersion { get; set; }

    [CommandSwitch("--sync-config")]
    public string? SyncConfig { get; set; }

    [CommandSwitch("--max-batch-size")]
    public int? MaxBatchSize { get; set; }

    [CommandSwitch("--runtime")]
    public string? Runtime { get; set; }

    [CommandSwitch("--code")]
    public string? Code { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}