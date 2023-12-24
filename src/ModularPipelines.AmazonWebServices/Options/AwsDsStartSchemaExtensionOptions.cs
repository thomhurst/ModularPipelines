using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "start-schema-extension")]
public record AwsDsStartSchemaExtensionOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--ldif-content")] string LdifContent,
[property: CommandSwitch("--description")] string Description
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}