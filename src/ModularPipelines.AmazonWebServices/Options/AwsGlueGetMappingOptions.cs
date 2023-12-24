using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "get-mapping")]
public record AwsGlueGetMappingOptions(
[property: CommandSwitch("--source")] string Source
) : AwsOptions
{
    [CommandSwitch("--sinks")]
    public string[]? Sinks { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}