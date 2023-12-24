using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "create-facet")]
public record AwsClouddirectoryCreateFacetOptions(
[property: CommandSwitch("--schema-arn")] string SchemaArn,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--attributes")]
    public string[]? Attributes { get; set; }

    [CommandSwitch("--object-type")]
    public string? ObjectType { get; set; }

    [CommandSwitch("--facet-style")]
    public string? FacetStyle { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}