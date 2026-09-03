using Microsoft.Extensions.Configuration;

namespace ModularPipelines.Distributed.UnitTests.Configuration;

public class DistributedOptionsTests
{
    [Test]
    public async Task ModuleResultTimeout_Defaults_To_Forty_Five_Minutes()
    {
        var options = new DistributedOptions();

        using (Assert.Multiple())
        {
            await Assert.That(options.CapabilityTimeout).IsEqualTo(TimeSpan.FromMinutes(5));
            await Assert.That(options.MinimumWorkerCount).IsEqualTo(0);
            await Assert.That(options.ModuleResultTimeout).IsEqualTo(TimeSpan.FromMinutes(45));
        }
    }

    [Test]
    public async Task Capabilities_CanBeBoundFromConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Distributed:Capabilities:0"] = "docker",
                ["Distributed:Capabilities:1"] = "gpu",
                ["Distributed:CapabilityTimeout"] = "00:00:30",
                ["Distributed:MinimumWorkerCount"] = "2",
            })
            .Build();
        var options = new DistributedOptions();

        configuration.GetSection("Distributed").Bind(options);

        using (Assert.Multiple())
        {
            await Assert.That(options.Capabilities)
                .IsEquivalentTo([Capability.Docker, Capability.Gpu]);
            await Assert.That(options.CapabilityTimeout).IsEqualTo(TimeSpan.FromSeconds(30));
            await Assert.That(options.MinimumWorkerCount).IsEqualTo(2);
        }
    }
}
