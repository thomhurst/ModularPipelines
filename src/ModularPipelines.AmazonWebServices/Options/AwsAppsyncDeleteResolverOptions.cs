using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "delete-resolver")]
public record AwsAppsyncDeleteResolverOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--type-name")] string TypeName,
[property: CliOption("--field-name")] string FieldName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}