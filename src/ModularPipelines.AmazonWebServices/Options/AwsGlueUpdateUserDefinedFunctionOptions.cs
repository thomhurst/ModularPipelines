using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "update-user-defined-function")]
public record AwsGlueUpdateUserDefinedFunctionOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--function-name")] string FunctionName,
[property: CliOption("--function-input")] string FunctionInput
) : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}