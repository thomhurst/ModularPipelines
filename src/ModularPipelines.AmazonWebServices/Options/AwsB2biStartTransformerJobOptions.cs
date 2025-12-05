using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("b2bi", "start-transformer-job")]
public record AwsB2biStartTransformerJobOptions(
[property: CliOption("--input-file")] string InputFile,
[property: CliOption("--output-location")] string OutputLocation,
[property: CliOption("--transformer-id")] string TransformerId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}