using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apigateway", "import-api-keys")]
public record AwsApigatewayImportApiKeysOptions(
[property: CliOption("--body")] string Body,
[property: CliOption("--format")] string Format
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}