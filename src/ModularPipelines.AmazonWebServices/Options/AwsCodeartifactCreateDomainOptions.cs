using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeartifact", "create-domain")]
public record AwsCodeartifactCreateDomainOptions(
[property: CommandSwitch("--domain")] string Domain
) : AwsOptions
{
    [CommandSwitch("--encryption-key")]
    public string? EncryptionKey { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}