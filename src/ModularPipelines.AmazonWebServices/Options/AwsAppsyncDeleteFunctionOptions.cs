using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "delete-function")]
public record AwsAppsyncDeleteFunctionOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--function-id")] string FunctionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}