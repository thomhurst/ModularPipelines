using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("schemas", "wait", "code-binding-exists")]
public record AwsSchemasWaitCodeBindingExistsOptions(
[property: CommandSwitch("--language")] string Language,
[property: CommandSwitch("--registry-name")] string RegistryName,
[property: CommandSwitch("--schema-name")] string SchemaName
) : AwsOptions
{
    [CommandSwitch("--schema-version")]
    public string? SchemaVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}