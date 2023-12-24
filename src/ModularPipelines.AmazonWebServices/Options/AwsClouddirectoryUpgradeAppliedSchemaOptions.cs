using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "upgrade-applied-schema")]
public record AwsClouddirectoryUpgradeAppliedSchemaOptions(
[property: CommandSwitch("--published-schema-arn")] string PublishedSchemaArn,
[property: CommandSwitch("--directory-arn")] string DirectoryArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}