using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "update-function")]
public record AwsCloudfrontUpdateFunctionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--if-match")] string IfMatch,
[property: CommandSwitch("--function-config")] string FunctionConfig,
[property: CommandSwitch("--function-code")] string FunctionCode
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}