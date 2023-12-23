using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "create-filter")]
public record AwsGuarddutyCreateFilterOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--finding-criteria")] string FindingCriteria
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--rank")]
    public int? Rank { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}