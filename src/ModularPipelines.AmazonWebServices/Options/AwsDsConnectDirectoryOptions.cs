using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "connect-directory")]
public record AwsDsConnectDirectoryOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--password")] string Password,
[property: CommandSwitch("--size")] string Size,
[property: CommandSwitch("--connect-settings")] string ConnectSettings
) : AwsOptions
{
    [CommandSwitch("--short-name")]
    public string? ShortName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}