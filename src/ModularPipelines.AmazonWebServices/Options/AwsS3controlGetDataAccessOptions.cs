using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "get-data-access")]
public record AwsS3controlGetDataAccessOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--target")] string Target,
[property: CommandSwitch("--permission")] string Permission
) : AwsOptions
{
    [CommandSwitch("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CommandSwitch("--privilege")]
    public string? Privilege { get; set; }

    [CommandSwitch("--target-type")]
    public string? TargetType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}