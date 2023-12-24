using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "put-parameter")]
public record AwsSsmPutParameterOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--value")] string Value
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--key-id")]
    public string? KeyId { get; set; }

    [CommandSwitch("--allowed-pattern")]
    public string? AllowedPattern { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--tier")]
    public string? Tier { get; set; }

    [CommandSwitch("--policies")]
    public string? Policies { get; set; }

    [CommandSwitch("--data-type")]
    public string? DataType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}