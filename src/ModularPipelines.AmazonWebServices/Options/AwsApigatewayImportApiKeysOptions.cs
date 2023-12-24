using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "import-api-keys")]
public record AwsApigatewayImportApiKeysOptions(
[property: CommandSwitch("--body")] string Body,
[property: CommandSwitch("--format")] string Format
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}