using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "create-type")]
public record AwsAppsyncCreateTypeOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--definition")] string Definition,
[property: CliOption("--format")] string Format
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}