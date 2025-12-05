using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "describe-conditional-forwarders")]
public record AwsDsDescribeConditionalForwardersOptions(
[property: CliOption("--directory-id")] string DirectoryId
) : AwsOptions
{
    [CliOption("--remote-domain-names")]
    public string[]? RemoteDomainNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}