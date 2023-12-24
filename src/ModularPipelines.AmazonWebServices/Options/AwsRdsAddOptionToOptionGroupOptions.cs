using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "add-option-to-option-group")]
public record AwsRdsAddOptionToOptionGroupOptions(
[property: CommandSwitch("--option-group-name")] string OptionGroupName
) : AwsOptions
{
    [CommandSwitch("--options")]
    public string[]? Options { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}