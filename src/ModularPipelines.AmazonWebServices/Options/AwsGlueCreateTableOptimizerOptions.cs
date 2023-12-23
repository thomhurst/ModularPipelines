using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "create-table-optimizer")]
public record AwsGlueCreateTableOptimizerOptions(
[property: CommandSwitch("--catalog-id")] string CatalogId,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--table-name")] string TableName,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--table-optimizer-configuration")] string TableOptimizerConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}