using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "delete-type")]
public record AwsAppsyncDeleteTypeOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--type-name")] string TypeName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}