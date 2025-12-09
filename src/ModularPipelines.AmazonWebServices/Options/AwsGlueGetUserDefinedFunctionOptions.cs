using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "get-user-defined-function")]
public record AwsGlueGetUserDefinedFunctionOptions(
[property: CliOption("--database-name")] string DatabaseName,
[property: CliOption("--function-name")] string FunctionName
) : AwsOptions
{
    [CliOption("--catalog-id")]
    public string? CatalogId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}