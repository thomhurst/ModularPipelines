using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "create-resolver")]
public record AwsAppsyncCreateResolverOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--type-name")] string TypeName,
[property: CliOption("--field-name")] string FieldName
) : AwsOptions
{
    [CliOption("--data-source-name")]
    public string? DataSourceName { get; set; }

    [CliOption("--request-mapping-template")]
    public string? RequestMappingTemplate { get; set; }

    [CliOption("--response-mapping-template")]
    public string? ResponseMappingTemplate { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--pipeline-config")]
    public string? PipelineConfig { get; set; }

    [CliOption("--sync-config")]
    public string? SyncConfig { get; set; }

    [CliOption("--caching-config")]
    public string? CachingConfig { get; set; }

    [CliOption("--max-batch-size")]
    public int? MaxBatchSize { get; set; }

    [CliOption("--runtime")]
    public string? Runtime { get; set; }

    [CliOption("--code")]
    public string? Code { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}