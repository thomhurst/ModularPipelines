using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "update-api-key")]
public record AwsAppsyncUpdateApiKeyOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--expires")]
    public long? Expires { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}