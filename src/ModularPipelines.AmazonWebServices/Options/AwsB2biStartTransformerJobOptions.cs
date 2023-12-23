using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("b2bi", "start-transformer-job")]
public record AwsB2biStartTransformerJobOptions(
[property: CommandSwitch("--input-file")] string InputFile,
[property: CommandSwitch("--output-location")] string OutputLocation,
[property: CommandSwitch("--transformer-id")] string TransformerId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}