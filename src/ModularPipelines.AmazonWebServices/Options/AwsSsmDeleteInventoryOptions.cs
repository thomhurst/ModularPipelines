using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "delete-inventory")]
public record AwsSsmDeleteInventoryOptions(
[property: CommandSwitch("--type-name")] string TypeName
) : AwsOptions
{
    [CommandSwitch("--schema-delete-option")]
    public string? SchemaDeleteOption { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}