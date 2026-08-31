using ModularPipelines.Google.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Google.UnitTests;

public class GcloudServiceAccountTests
{
    [Test]
    public async Task RunDeployRendersServiceAccountNamesAndEmails()
    {
        var arguments = BuildArguments(new GcloudRunDeployOptions
        {
            ServiceAccountValue = "runtime@project.iam.gserviceaccount.com",
            BuildServiceAccountValue =
                "projects/project/serviceAccounts/build@project.iam.gserviceaccount.com",
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "--service-account=runtime@project.iam.gserviceaccount.com",
            "--build-service-account="
            + "projects/project/serviceAccounts/build@project.iam.gserviceaccount.com",
        ]);
    }
}
