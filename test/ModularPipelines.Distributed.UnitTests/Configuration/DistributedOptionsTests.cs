using Microsoft.Extensions.Configuration;

namespace ModularPipelines.Distributed.UnitTests.Configuration;

public class DistributedOptionsTests
{
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
