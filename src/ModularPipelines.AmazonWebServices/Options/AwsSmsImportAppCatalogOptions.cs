using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sms", "import-app-catalog")]
public record AwsSmsImportAppCatalogOptions : AwsOptions
{
    [CommandSwitch("--role-name")]
    public string? RoleName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}