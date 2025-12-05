using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "create-access")]
public record AwsTransferCreateAccessOptions(
[property: CliOption("--role")] string Role,
[property: CliOption("--server-id")] string ServerId,
[property: CliOption("--external-id")] string ExternalId
) : AwsOptions
{
    [CliOption("--home-directory")]
    public string? HomeDirectory { get; set; }

    [CliOption("--home-directory-type")]
    public string? HomeDirectoryType { get; set; }

    [CliOption("--home-directory-mappings")]
    public string[]? HomeDirectoryMappings { get; set; }

    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--posix-profile")]
    public string? PosixProfile { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}