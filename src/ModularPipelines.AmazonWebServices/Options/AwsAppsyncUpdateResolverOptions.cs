using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "update-resolver")]
public record AwsAppsyncUpdateResolverOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--type-name")] string TypeName,
[property: CommandSwitch("--field-name")] string FieldName
) : AwsOptions
{
    [CommandSwitch("--data-source-name")]
    public string? DataSourceName { get; set; }

    [CommandSwitch("--request-mapping-template")]
    public string? RequestMappingTemplate { get; set; }

    [CommandSwitch("--response-mapping-template")]
    public string? ResponseMappingTemplate { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--pipeline-config")]
    public string? PipelineConfig { get; set; }

    [CommandSwitch("--sync-config")]
    public string? SyncConfig { get; set; }

    [CommandSwitch("--caching-config")]
    public string? CachingConfig { get; set; }

    [CommandSwitch("--max-batch-size")]
    public int? MaxBatchSize { get; set; }

    [CommandSwitch("--runtime")]
    public string? Runtime { get; set; }

    [CommandSwitch("--code")]
    public string? Code { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}