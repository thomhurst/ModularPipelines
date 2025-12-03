using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar-connections", "create-host")]
public record AwsCodestarConnectionsCreateHostOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--provider-type")] string ProviderType,
[property: CliOption("--provider-endpoint")] string ProviderEndpoint
) : AwsOptions
{
    [CliOption("--vpc-configuration")]
    public string? VpcConfiguration { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}