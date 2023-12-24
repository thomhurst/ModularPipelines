using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "create-access")]
public record AwsTransferCreateAccessOptions(
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--server-id")] string ServerId,
[property: CommandSwitch("--external-id")] string ExternalId
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

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}