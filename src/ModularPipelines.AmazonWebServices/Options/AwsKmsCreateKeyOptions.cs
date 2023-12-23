using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "create-key")]
public record AwsKmsCreateKeyOptions : AwsOptions
{
    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--key-usage")]
    public string? KeyUsage { get; set; }

    [CommandSwitch("--customer-master-key-spec")]
    public string? CustomerMasterKeySpec { get; set; }

    [CommandSwitch("--key-spec")]
    public string? KeySpec { get; set; }

    [CommandSwitch("--origin")]
    public string? Origin { get; set; }

    [CommandSwitch("--custom-key-store-id")]
    public string? CustomKeyStoreId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--xks-key-id")]
    public string? XksKeyId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}