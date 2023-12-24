using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "get-schema-as-json")]
public record AwsClouddirectoryGetSchemaAsJsonOptions(
[property: CommandSwitch("--schema-arn")] string SchemaArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}