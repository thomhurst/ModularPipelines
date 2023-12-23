using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguru-security", "batch-get-findings")]
public record AwsCodeguruSecurityBatchGetFindingsOptions(
[property: CommandSwitch("--finding-identifiers")] string[] FindingIdentifiers
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}