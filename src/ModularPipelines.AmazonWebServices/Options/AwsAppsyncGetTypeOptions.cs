using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "get-type")]
public record AwsAppsyncGetTypeOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--type-name")] string TypeName,
[property: CommandSwitch("--format")] string Format
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}