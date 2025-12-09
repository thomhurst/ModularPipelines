using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "update-facet")]
public record AwsClouddirectoryUpdateFacetOptions(
[property: CliOption("--schema-arn")] string SchemaArn,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--attribute-updates")]
    public string[]? AttributeUpdates { get; set; }

    [CliOption("--object-type")]
    public string? ObjectType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}