using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "test-function")]
public record AwsCloudfrontTestFunctionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--if-match")] string IfMatch,
[property: CommandSwitch("--event-object")] string EventObject
) : AwsOptions
{
    [CommandSwitch("--stage")]
    public string? Stage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}