using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "create-user-defined-function")]
public record AwsGlueCreateUserDefinedFunctionOptions(
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--function-input")] string FunctionInput
) : AwsOptions
{
    [CommandSwitch("--catalog-id")]
    public string? CatalogId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}