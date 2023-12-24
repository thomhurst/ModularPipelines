using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotfleetwise", "import-decoder-manifest")]
public record AwsIotfleetwiseImportDecoderManifestOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--network-file-definitions")] string[] NetworkFileDefinitions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}