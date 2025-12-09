using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifyuibuilder", "exchange-code-for-token")]
public record AwsAmplifyuibuilderExchangeCodeForTokenOptions(
[property: CliOption("--provider")] string Provider,
[property: CliOption("--request")] string Request
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}