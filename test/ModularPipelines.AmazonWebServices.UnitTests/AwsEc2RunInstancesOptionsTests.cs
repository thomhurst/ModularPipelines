using ModularPipelines.AmazonWebServices.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.UnitTests;

public class AwsEc2RunInstancesOptionsTests
{
    [Test]
    public async Task Public_Ip_Association_Emits_Both_Explicit_Forms()
    {
        var enabled = BuildArguments(new AwsEc2RunInstancesOptions
        {
            AssociatePublicIpAddress = true,
        });
        var disabled = BuildArguments(new AwsEc2RunInstancesOptions
        {
            AssociatePublicIpAddress = false,
        });
        var unspecified = BuildArguments(new AwsEc2RunInstancesOptions());

        using (Assert.Multiple())
        {
            await Assert.That(enabled).IsEquivalentTo(["--associate-public-ip-address"], TUnit.Assertions.Enums.CollectionOrdering.Matching);
            await Assert.That(disabled).IsEquivalentTo(["--no-associate-public-ip-address"], TUnit.Assertions.Enums.CollectionOrdering.Matching);
            await Assert.That(unspecified).IsEmpty();
        }
    }
}
