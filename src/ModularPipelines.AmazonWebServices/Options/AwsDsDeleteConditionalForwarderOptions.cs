using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "delete-conditional-forwarder")]
public record AwsDsDeleteConditionalForwarderOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--remote-domain-name")] string RemoteDomainName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}