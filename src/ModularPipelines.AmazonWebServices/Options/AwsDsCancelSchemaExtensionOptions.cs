using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "cancel-schema-extension")]
public record AwsDsCancelSchemaExtensionOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--schema-extension-id")] string SchemaExtensionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}