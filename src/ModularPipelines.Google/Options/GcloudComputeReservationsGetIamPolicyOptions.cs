using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "reservations", "get-iam-policy")]
public record GcloudComputeReservationsGetIamPolicyOptions(
[property: PositionalArgument] string Reservation,
[property: PositionalArgument] string Zone
) : GcloudOptions;