using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "create-directory")]
public record AwsDsCreateDirectoryOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--password")] string Password,
[property: CommandSwitch("--size")] string Size
) : AwsOptions
{
    [CommandSwitch("--short-name")]
    public string? ShortName { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--vpc-settings")]
    public string? VpcSettings { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}