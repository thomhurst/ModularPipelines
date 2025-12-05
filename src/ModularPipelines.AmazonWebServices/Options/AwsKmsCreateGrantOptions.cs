using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kms", "create-grant")]
public record AwsKmsCreateGrantOptions(
[property: CliOption("--key-id")] string KeyId,
[property: CliOption("--grantee-principal")] string GranteePrincipal,
[property: CliOption("--operations")] string[] Operations
) : AwsOptions
{
    [CliOption("--retiring-principal")]
    public string? RetiringPrincipal { get; set; }

    [CliOption("--constraints")]
    public string? Constraints { get; set; }

    [CliOption("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}