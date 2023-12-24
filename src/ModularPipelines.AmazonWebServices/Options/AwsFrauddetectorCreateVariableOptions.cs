using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "create-variable")]
public record AwsFrauddetectorCreateVariableOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--data-type")] string DataType,
[property: CommandSwitch("--data-source")] string DataSource,
[property: CommandSwitch("--default-value")] string DefaultValue
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--variable-type")]
    public string? VariableType { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}