using ModularPipelines.Google.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Google.UnitTests;

public class GcloudPubsubServiceAccountTests
{
    [Test]
    public async Task IngestionServiceAccountsRenderNamesAndEmails()
    {
        var arguments = BuildArguments(new GcloudPubsubTopicsUpdateOptions
        {
            AwsMskIngestionServiceAccount = "msk-importer",
            AzureEventHubsIngestionServiceAccount =
                "azure-ingestion@project.iam.gserviceaccount.com",
            ConfluentCloudIngestionServiceAccount = "confluent-importer",
            KinesisIngestionServiceAccount =
                "kinesis-ingestion@project.iam.gserviceaccount.com",
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "--aws-msk-ingestion-service-account=msk-importer",
            "--azure-event-hubs-ingestion-service-account="
            + "azure-ingestion@project.iam.gserviceaccount.com",
            "--confluent-cloud-ingestion-service-account=confluent-importer",
            "--kinesis-ingestion-service-account="
            + "kinesis-ingestion@project.iam.gserviceaccount.com",
        ]);
    }
}
