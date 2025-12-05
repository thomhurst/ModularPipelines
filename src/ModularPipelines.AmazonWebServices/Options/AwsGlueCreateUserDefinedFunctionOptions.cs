using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "create-user-defined-function")]
public record AwsGlueCreateUserDefinedFunctionOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--function-input")] string FunctionInput
) : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}