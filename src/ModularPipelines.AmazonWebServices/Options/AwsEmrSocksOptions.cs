using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "socks")]
public record AwsEmrSocksOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--key-pair-file")] string KeyPairFile
) : AwsOptions;