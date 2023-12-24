using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "list-hub-content-versions")]
public record AwsSagemakerListHubContentVersionsOptions(
[property: CommandSwitch("--hub-name")] string HubName,
[property: CommandSwitch("--hub-content-type")] string HubContentType,
[property: CommandSwitch("--hub-content-name")] string HubContentName
) : AwsOptions
{
    [CommandSwitch("--min-version")]
    public string? MinVersion { get; set; }

    [CommandSwitch("--max-schema-version")]
    public string? MaxSchemaVersion { get; set; }

    [CommandSwitch("--creation-time-before")]
    public long? CreationTimeBefore { get; set; }

    [CommandSwitch("--creation-time-after")]
    public long? CreationTimeAfter { get; set; }

    [CommandSwitch("--sort-by")]
    public string? SortBy { get; set; }

    [CommandSwitch("--sort-order")]
    public string? SortOrder { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}