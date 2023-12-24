using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "copy-option-group")]
public record AwsRdsCopyOptionGroupOptions(
[property: CommandSwitch("--source-option-group-identifier")] string SourceOptionGroupIdentifier,
[property: CommandSwitch("--target-option-group-identifier")] string TargetOptionGroupIdentifier,
[property: CommandSwitch("--target-option-group-description")] string TargetOptionGroupDescription
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}