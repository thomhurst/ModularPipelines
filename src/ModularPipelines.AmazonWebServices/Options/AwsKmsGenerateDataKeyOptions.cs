using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "generate-data-key")]
public record AwsKmsGenerateDataKeyOptions(
[property: CommandSwitch("--key-id")] string KeyId
) : AwsOptions
{
    [CommandSwitch("--encryption-context")]
    public IEnumerable<KeyValue>? EncryptionContext { get; set; }

    [CommandSwitch("--number-of-bytes")]
    public int? NumberOfBytes { get; set; }

    [CommandSwitch("--key-spec")]
    public string? KeySpec { get; set; }

    [CommandSwitch("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CommandSwitch("--recipient")]
    public string? Recipient { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}