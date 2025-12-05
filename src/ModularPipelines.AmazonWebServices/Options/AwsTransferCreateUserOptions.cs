using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "create-user")]
public record AwsTransferCreateUserOptions(
[property: CliOption("--role")] string Role,
[property: CliOption("--server-id")] string ServerId,
[property: CliOption("--user-name")] string UserName
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

    [CliOption("--ssh-public-key-body")]
    public string? SshPublicKeyBody { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}