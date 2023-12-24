using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "put-schema-from-json")]
public record AwsClouddirectoryPutSchemaFromJsonOptions(
[property: CommandSwitch("--schema-arn")] string SchemaArn,
[property: CommandSwitch("--document")] string Document
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}