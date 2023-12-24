using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "get-function")]
public record AwsCloudfrontGetFunctionOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--stage")]
    public string? Stage { get; set; }
}