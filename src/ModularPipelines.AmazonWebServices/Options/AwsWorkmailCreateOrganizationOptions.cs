using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "create-organization")]
public record AwsWorkmailCreateOrganizationOptions(
[property: CommandSwitch("--alias")] string Alias
) : AwsOptions
{
    [CommandSwitch("--directory-id")]
    public string? DirectoryId { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--domains")]
    public string[]? Domains { get; set; }

    [CommandSwitch("--kms-key-arn")]
    public string? KmsKeyArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}