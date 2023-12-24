using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "get-parameters-for-import")]
public record AwsKmsGetParametersForImportOptions(
[property: CommandSwitch("--key-id")] string KeyId,
[property: CommandSwitch("--wrapping-algorithm")] string WrappingAlgorithm,
[property: CommandSwitch("--wrapping-key-spec")] string WrappingKeySpec
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}