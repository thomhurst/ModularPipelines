using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "delete-function")]
public record AwsCloudfrontDeleteFunctionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--if-match")] string IfMatch
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}