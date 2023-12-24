using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "create-option-group")]
public record AwsRdsCreateOptionGroupOptions(
[property: CommandSwitch("--option-group-name")] string OptionGroupName,
[property: CommandSwitch("--engine-name")] string EngineName,
[property: CommandSwitch("--major-engine-version")] string MajorEngineVersion,
[property: CommandSwitch("--option-group-description")] string OptionGroupDescription
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}