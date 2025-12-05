using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "put")]
public record AwsEmrPutOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--key-pair-file")] string KeyPairFile,
[property: CliOption("--src")] string Src
) : AwsOptions
{
    [CliOption("--dest")]
    public string? Dest { get; set; }
}