using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rolesanywhere", "import-crl")]
public record AwsRolesanywhereImportCrlOptions(
[property: CommandSwitch("--crl-data")] string CrlData,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--trust-anchor-arn")] string TrustAnchorArn
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}