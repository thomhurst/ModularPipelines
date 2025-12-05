using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgn", "associate-source-servers")]
public record AwsMgnAssociateSourceServersOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--source-server-ids")] string[] SourceServerIds
) : AwsOptions
{
    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}