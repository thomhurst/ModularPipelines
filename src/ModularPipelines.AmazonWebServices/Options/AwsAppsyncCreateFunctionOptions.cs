using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "create-function")]
public record AwsAppsyncCreateFunctionOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--name")] string Name,
[property: CliOption("--data-source-name")] string DataSourceName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--request-mapping-template")]
    public string? RequestMappingTemplate { get; set; }

    [CliOption("--response-mapping-template")]
    public string? ResponseMappingTemplate { get; set; }

    [CliOption("--function-version")]
    public string? FunctionVersion { get; set; }

    [CliOption("--sync-config")]
    public string? SyncConfig { get; set; }

    [CliOption("--max-batch-size")]
    public int? MaxBatchSize { get; set; }

    [CliOption("--runtime")]
    public string? Runtime { get; set; }

    [CliOption("--code")]
    public string? Code { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}