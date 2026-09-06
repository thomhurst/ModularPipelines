using ModularPipelines.Distributed.SignalR;

namespace ModularPipelines.Distributed.SignalR.UnitTests;

public class SignalRNamespaceTests
{
    [Test]
    public async Task Consumer_Entry_Points_Are_In_The_Package_Root()
    {
        await Assert.That(typeof(SignalRDistributedExtensions).Namespace)
            .IsEqualTo("ModularPipelines.Distributed.SignalR");
        await Assert.That(typeof(SignalRDistributedOptions).Namespace)
            .IsEqualTo("ModularPipelines.Distributed.SignalR");
    }
}
