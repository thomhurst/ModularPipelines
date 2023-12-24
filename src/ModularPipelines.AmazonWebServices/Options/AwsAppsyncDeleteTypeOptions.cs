using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "delete-type")]
public record AwsAppsyncDeleteTypeOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--type-name")] string TypeName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}