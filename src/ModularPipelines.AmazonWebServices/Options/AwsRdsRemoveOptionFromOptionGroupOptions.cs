using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "remove-option-from-option-group")]
public record AwsRdsRemoveOptionFromOptionGroupOptions(
[property: CommandSwitch("--option-group-name")] string OptionGroupName
) : AwsOptions
{
    [CommandSwitch("--options")]
    public string[]? Options { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}