using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "update-facet")]
public record AwsClouddirectoryUpdateFacetOptions(
[property: CommandSwitch("--schema-arn")] string SchemaArn,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--attribute-updates")]
    public string[]? AttributeUpdates { get; set; }

    [CommandSwitch("--object-type")]
    public string? ObjectType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}