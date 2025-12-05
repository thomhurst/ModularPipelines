using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "get-type")]
public record AwsAppsyncGetTypeOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--type-name")] string TypeName,
[property: CliOption("--format")] string Format
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}