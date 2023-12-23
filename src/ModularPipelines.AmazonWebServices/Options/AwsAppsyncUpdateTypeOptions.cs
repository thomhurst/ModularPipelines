using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "update-type")]
public record AwsAppsyncUpdateTypeOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--type-name")] string TypeName,
[property: CommandSwitch("--format")] string Format
) : AwsOptions
{
    [CommandSwitch("--definition")]
    public string? Definition { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}