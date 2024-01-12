using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "list-testable-permissions")]
public record GcloudIamListTestablePermissionsOptions(
[property: PositionalArgument] string Resource
) : GcloudOptions;