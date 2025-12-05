using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearchserverless", "create-security-policy")]
public record AwsOpensearchserverlessCreateSecurityPolicyOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--policy")] string Policy,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}