using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "list-functions-by-code-signing-config")]
public record AwsLambdaListFunctionsByCodeSigningConfigOptions(
[property: CliOption("--code-signing-config-arn")] string CodeSigningConfigArn
) : AwsOptions
{
    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}