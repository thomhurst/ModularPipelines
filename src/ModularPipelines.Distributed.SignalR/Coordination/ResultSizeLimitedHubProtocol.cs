using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Protocol;
using ModularPipelines.Distributed.Serialization;
using ModularPipelines.Distributed.SignalR.Hub;

namespace ModularPipelines.Distributed.SignalR.Coordination;

/// <summary>
/// Checks the actual invocation, including its ID, headers, JSON escaping and record separator,
/// before any bytes reach the transport. Only worker result publications are replaced.
/// </summary>
internal sealed class ResultSizeLimitedHubProtocol(IHubProtocol inner, long maxMessageSizeBytes) : IHubProtocol
{
    private readonly ModuleResultSerializer _serializer = new(new ModuleTypeRegistry());

    public string Name => inner.Name;

    public int Version => inner.Version;

    public TransferFormat TransferFormat => inner.TransferFormat;

    public bool IsVersionSupported(int version) => inner.IsVersionSupported(version);

    public bool TryParseMessage(ref ReadOnlySequence<byte> input, IInvocationBinder binder,
        [NotNullWhen(true)] out HubMessage? message) => inner.TryParseMessage(ref input, binder, out message);

    public void WriteMessage(HubMessage message, IBufferWriter<byte> output)
    {
        if (message is InvocationMessage { Target: HubMethodNames.PublishResult })
        {
            output.Write(GetMessageBytes(message).Span);
            return;
        }

        inner.WriteMessage(message, output);
    }

    public ReadOnlyMemory<byte> GetMessageBytes(HubMessage message)
    {
        var bytes = inner.GetMessageBytes(message);
        if (message is not InvocationMessage { Target: HubMethodNames.PublishResult } invocation
            || bytes.Length <= maxMessageSizeBytes)
        {
            return bytes;
        }

        var result = (SerializedModuleResult) invocation.Arguments[0]!;
        var error = $"SignalR result invocation is {bytes.Length} bytes, exceeding "
                    + $"{nameof(SignalRDistributedOptions)}.{nameof(SignalRDistributedOptions.MaxMessageSizeBytes)} "
                    + $"({maxMessageSizeBytes} bytes). Reduce module output or increase this option on every process.";
        var failure = _serializer.SerializeFailure(result.ModuleId, new InvalidOperationException(error), result.WorkerId) with
        {
            CommandCount = result.CommandCount,
        };
        var lease = (ModuleLease?) invocation.Arguments[1];
        // The hub needs only lease identity. Do not echo large dependency or artifact metadata.
        var compactLease = lease is null ? null : lease with
        {
            Assignment = new ModuleAssignment
            {
                ModuleId = lease.Assignment.ModuleId,
                RequiredCapabilities = CapabilityRequirement.None,
                PipelineSchemaVersion = string.Empty,
            },
        };
        var replacement = new InvocationMessage(invocation.InvocationId, invocation.Target, [failure, compactLease])
        {
            Headers = invocation.Headers,
        };
        var failureBytes = inner.GetMessageBytes(replacement);
        if (failureBytes.Length > maxMessageSizeBytes)
        {
            // Some limits cannot even carry the mandatory identities and a failure. Never send
            // another oversized message or turn this configuration error into a reconnect loop.
            throw new HubException(error + $" The failure invocation also requires {failureBytes.Length} bytes.");
        }

        return failureBytes;
    }
}
