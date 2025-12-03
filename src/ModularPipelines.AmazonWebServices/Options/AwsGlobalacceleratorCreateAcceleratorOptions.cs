using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("globalaccelerator", "create-accelerator")]
public record AwsGlobalacceleratorCreateAcceleratorOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--ip-address-type")]
    public string? IpAddressType { get; set; }

    [CliOption("--ip-addresses")]
    public string[]? IpAddresses { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}