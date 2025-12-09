using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "socks")]
public record AwsEmrSocksOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--key-pair-file")] string KeyPairFile
) : AwsOptions;