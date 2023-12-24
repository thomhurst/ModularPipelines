using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplifyuibuilder", "exchange-code-for-token")]
public record AwsAmplifyuibuilderExchangeCodeForTokenOptions(
[property: CommandSwitch("--provider")] string Provider,
[property: CommandSwitch("--request")] string Request
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}