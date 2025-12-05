using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amp", "create-workspace")]
public record AwsAmpCreateWorkspaceOptions : AwsOptions
{
    [CliOption("--alias")]
    public string? Alias { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}