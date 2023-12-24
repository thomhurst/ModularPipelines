using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "create-configured-table")]
public record AwsCleanroomsCreateConfiguredTableOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--table-reference")] string TableReference,
[property: CommandSwitch("--allowed-columns")] string[] AllowedColumns,
[property: CommandSwitch("--analysis-method")] string AnalysisMethod
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}