using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("textract", "update-adapter")]
public record AwsTextractUpdateAdapterOptions(
[property: CommandSwitch("--adapter-id")] string AdapterId
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--adapter-name")]
    public string? AdapterName { get; set; }

    [CommandSwitch("--auto-update")]
    public string? AutoUpdate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}