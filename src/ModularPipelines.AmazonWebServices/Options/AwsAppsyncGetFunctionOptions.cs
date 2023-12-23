using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "get-function")]
public record AwsAppsyncGetFunctionOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--function-id")] string FunctionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}