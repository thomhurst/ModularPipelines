using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("personalize", "create-schema")]
public record AwsPersonalizeCreateSchemaOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--schema")] string Schema
) : AwsOptions
{
    [CommandSwitch("--domain")]
    public string? Domain { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}