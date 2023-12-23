using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "create-user")]
public record AwsTransferCreateUserOptions(
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--server-id")] string ServerId,
[property: CommandSwitch("--user-name")] string UserName
) : AwsOptions
{
    [CommandSwitch("--home-directory")]
    public string? HomeDirectory { get; set; }

    [CommandSwitch("--home-directory-type")]
    public string? HomeDirectoryType { get; set; }

    [CommandSwitch("--home-directory-mappings")]
    public string[]? HomeDirectoryMappings { get; set; }

    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--posix-profile")]
    public string? PosixProfile { get; set; }

    [CommandSwitch("--ssh-public-key-body")]
    public string? SshPublicKeyBody { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}