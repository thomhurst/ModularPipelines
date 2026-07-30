using Microsoft.Extensions.Configuration;

namespace ModularPipelines.Distributed.UnitTests.Configuration;

public class DistributedOptionsTests
{
    [Test]
    public async Task ModuleResultTimeout_Defaults_To_Forty_Five_Minutes()
    {
        var options = new DistributedOptions();

        await Assert.That(options.ModuleResultTimeoutSeconds).IsEqualTo(2700);
    }

    [Test]
    public async Task Capabilities_CanBeBoundFromConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Distributed:Capabilities:0"] = "docker",
                ["Distributed:Capabilities:1"] = "gpu",
            })
            .Build();
        var options = new DistributedOptions();

        configuration.GetSection("Distributed").Bind(options);

        await Assert.That(options.Capabilities).IsEquivalentTo(["docker", "gpu"]);
    }
}
