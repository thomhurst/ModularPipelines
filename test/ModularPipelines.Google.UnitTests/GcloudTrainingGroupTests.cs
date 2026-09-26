using System.ComponentModel.DataAnnotations;
using ModularPipelines.Google.Options;

namespace ModularPipelines.Google.UnitTests;

public class GcloudTrainingGroupTests
{
    [Test]
    public async Task Training_Validates_All_Selections_Of_Independent_Groups()
    {
        // Exhaust the eight documented flags: KMS selectors need a key; machine pairs are all-or-none.
        for (var selection = 0; selection < 256; selection++)
        {
            var options = new GcloudAiPlatformJobsSubmitTrainingOptions("job")
            {
                KmsKey = (selection & 1) != 0 ? "key" : null,
                KmsKeyring = (selection & 2) != 0 ? "ring" : null,
                KmsLocation = (selection & 4) != 0 ? "us-central1" : null,
                KmsProject = (selection & 8) != 0 ? "project" : null,
                ParameterServerCount = (selection & 16) != 0 ? 1 : null,
                ParameterServerMachineType = (selection & 32) != 0 ? "n1-standard-4" : null,
                WorkerCount = (selection & 64) != 0 ? 1 : null,
                WorkerMachineType = (selection & 128) != 0 ? "n1-standard-4" : null,
            };
            var keyValid = (selection & 14) == 0 || (selection & 1) != 0;
            var serversValid = (selection & 48) is 0 or 48;
            var workersValid = (selection & 192) is 0 or 192;
            var errors = new List<ValidationResult>();

            await Assert.That(Validator.TryValidateObject(options, new(options), errors, true))
                .IsEqualTo(keyValid && serversValid && workersValid)
                .Because($"Training selection {selection}: {string.Join("; ", errors)}");
        }
    }
}
