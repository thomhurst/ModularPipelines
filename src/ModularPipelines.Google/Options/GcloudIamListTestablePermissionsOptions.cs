using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "list-testable-permissions")]
public record GcloudIamListTestablePermissionsOptions(
[property: CliArgument] string Resource
) : GcloudOptions;