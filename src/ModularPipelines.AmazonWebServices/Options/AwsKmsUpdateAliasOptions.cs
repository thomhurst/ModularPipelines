using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "update-alias")]
public record AwsKmsUpdateAliasOptions(
[property: CliOption("--alias-name")] string AliasName,
[property: CliOption("--target-key-id")] string TargetKeyId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}