using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "retire-grant")]
public record AwsKmsRetireGrantOptions : AwsOptions
{
    [CliOption("--grant-token")]
    public string? GrantToken { get; set; }

    [CliOption("--key-id")]
    public string? KeyId { get; set; }

    [CliOption("--grant-id")]
    public string? GrantId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}