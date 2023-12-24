using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "create-typed-link-facet")]
public record AwsClouddirectoryCreateTypedLinkFacetOptions(
[property: CommandSwitch("--schema-arn")] string SchemaArn,
[property: CommandSwitch("--facet")] string Facet
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}