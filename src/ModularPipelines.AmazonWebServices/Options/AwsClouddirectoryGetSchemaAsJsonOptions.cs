using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "get-schema-as-json")]
public record AwsClouddirectoryGetSchemaAsJsonOptions(
[property: CliOption("--schema-arn")] string SchemaArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}