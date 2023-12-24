using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "update-user-defined-function")]
public record AwsGlueUpdateUserDefinedFunctionOptions(
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--function-name")] string FunctionName,
[property: CommandSwitch("--function-input")] string FunctionInput
) : AwsOptions
{
    [CommandSwitch("--catalog-id")]
    public string? CatalogId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}