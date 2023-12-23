using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "start-reference-import-job")]
public record AwsOmicsStartReferenceImportJobOptions(
[property: CommandSwitch("--reference-store-id")] string ReferenceStoreId,
[property: CommandSwitch("--role-arn")] string RoleArn,
[property: CommandSwitch("--sources")] string[] Sources
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}