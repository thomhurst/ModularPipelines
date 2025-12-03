using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotfleetwise", "import-decoder-manifest")]
public record AwsIotfleetwiseImportDecoderManifestOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--network-file-definitions")] string[] NetworkFileDefinitions
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}