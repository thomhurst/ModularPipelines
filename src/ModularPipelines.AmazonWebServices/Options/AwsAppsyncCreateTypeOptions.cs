using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "create-type")]
public record AwsAppsyncCreateTypeOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--definition")] string Definition,
[property: CommandSwitch("--format")] string Format
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}