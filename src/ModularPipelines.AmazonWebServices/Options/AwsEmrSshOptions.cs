using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "ssh")]
public record AwsEmrSshOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--key-pair-file")] string KeyPairFile
) : AwsOptions
{
    [CliOption("--command")]
    public string? Command { get; set; }
}