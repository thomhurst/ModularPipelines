using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securityhub", "update-security-control")]
public record AwsSecurityhubUpdateSecurityControlOptions(
[property: CommandSwitch("--security-control-id")] string SecurityControlId,
[property: CommandSwitch("--parameters")] IEnumerable<KeyValue> Parameters
) : AwsOptions
{
    [CommandSwitch("--last-update-reason")]
    public string? LastUpdateReason { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}