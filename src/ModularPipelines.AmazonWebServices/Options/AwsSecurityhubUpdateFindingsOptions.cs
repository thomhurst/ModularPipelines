using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "update-findings")]
public record AwsSecurityhubUpdateFindingsOptions(
[property: CommandSwitch("--filters")] string Filters
) : AwsOptions
{
    [CommandSwitch("--note")]
    public string? Note { get; set; }

    [CommandSwitch("--record-state")]
    public string? RecordState { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}