using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "generate-random")]
public record AwsKmsGenerateRandomOptions : AwsOptions
{
    [CommandSwitch("--number-of-bytes")]
    public int? NumberOfBytes { get; set; }

    [CommandSwitch("--custom-key-store-id")]
    public string? CustomKeyStoreId { get; set; }

    [CommandSwitch("--recipient")]
    public string? Recipient { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}