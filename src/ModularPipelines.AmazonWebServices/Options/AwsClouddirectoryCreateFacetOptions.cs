using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("clouddirectory", "create-facet")]
public record AwsClouddirectoryCreateFacetOptions(
[property: CliOption("--schema-arn")] string SchemaArn,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--attributes")]
    public string[]? Attributes { get; set; }

    [CliOption("--object-type")]
    public string? ObjectType { get; set; }

    [CliOption("--facet-style")]
    public string? FacetStyle { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}