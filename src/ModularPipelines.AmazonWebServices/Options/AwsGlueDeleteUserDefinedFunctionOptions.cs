using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "delete-user-defined-function")]
public record AwsGlueDeleteUserDefinedFunctionOptions(
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--function-name")] string FunctionName
) : AwsOptions
{
    [CommandSwitch("--catalog-id")]
    public string? CatalogId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}