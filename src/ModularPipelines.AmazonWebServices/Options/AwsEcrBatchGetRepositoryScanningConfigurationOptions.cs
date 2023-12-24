using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecr", "batch-get-repository-scanning-configuration")]
public record AwsEcrBatchGetRepositoryScanningConfigurationOptions(
[property: CommandSwitch("--repository-names")] string[] RepositoryNames
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}