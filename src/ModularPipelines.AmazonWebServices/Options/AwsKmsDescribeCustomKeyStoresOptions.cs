using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "describe-custom-key-stores")]
public record AwsKmsDescribeCustomKeyStoresOptions : AwsOptions
{
    [CommandSwitch("--custom-key-store-id")]
    public string? CustomKeyStoreId { get; set; }

    [CommandSwitch("--custom-key-store-name")]
    public string? CustomKeyStoreName { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}