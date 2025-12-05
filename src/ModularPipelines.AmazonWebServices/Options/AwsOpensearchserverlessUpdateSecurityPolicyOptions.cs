using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearchserverless", "update-security-policy")]
public record AwsOpensearchserverlessUpdateSecurityPolicyOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--policy-version")] string PolicyVersion,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}