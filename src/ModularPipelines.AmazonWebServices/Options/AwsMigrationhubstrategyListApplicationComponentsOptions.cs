using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("migrationhubstrategy", "list-application-components")]
public record AwsMigrationhubstrategyListApplicationComponentsOptions : AwsOptions
{
    [CommandSwitch("--application-component-criteria")]
    public string? ApplicationComponentCriteria { get; set; }

    [CommandSwitch("--filter-value")]
    public string? FilterValue { get; set; }

    [CommandSwitch("--group-id-filter")]
    public string[]? GroupIdFilter { get; set; }

    [CommandSwitch("--sort")]
    public string? Sort { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}