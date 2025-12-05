using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "put-schema-from-json")]
public record AwsClouddirectoryPutSchemaFromJsonOptions(
[property: CliOption("--schema-arn")] string SchemaArn,
[property: CliOption("--document")] string Document
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}