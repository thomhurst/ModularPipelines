using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "get-filter")]
public record AwsGuarddutyGetFilterOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--filter-name")] string FilterName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}