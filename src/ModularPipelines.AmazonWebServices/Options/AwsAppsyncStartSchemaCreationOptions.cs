using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appsync", "start-schema-creation")]
public record AwsAppsyncStartSchemaCreationOptions(
[property: CliOption("--api-id")] string ApiId,
[property: CliOption("--definition")] string Definition
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}