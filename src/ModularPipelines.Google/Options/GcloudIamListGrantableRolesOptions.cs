using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "list-grantable-roles")]
public record GcloudIamListGrantableRolesOptions(
[property: PositionalArgument] string Resource
) : GcloudOptions;