using System.ComponentModel.DataAnnotations;
using ModularPipelines.Google.Enums;
using ModularPipelines.Google.Options;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Google.UnitTests;

public class GcloudWorkbenchExecutionTests
{
    [Test]
    [Arguments("default", true)]
    [Arguments("machine", true)]
    [Arguments("accelerator", false)]
    [Arguments("accelerator-count", true)]
    [Arguments("disk-size", false)]
    [Arguments("disk-size-type", true)]
    [Arguments("no-internet", true)]
    [Arguments("no-source", false)]
    public async Task Optional_Compute_Settings_Preserve_Their_Companion_Requirements(string selection, bool valid)
    {
        var options = new GcloudWorkbenchExecutionsCreateOptions("region", "execution", "gs://output", "service-account")
        {
            GcsNotebookUri = selection == "no-source" ? null : "gs://notebook.ipynb",
            MachineType = selection == "machine" ? "e2-standard-4" : null,
            AcceleratorType = selection is "accelerator" or "accelerator-count"
                ? GcloudWorkbenchExecutionsCreateAcceleratorType.NvidiaL4 : null,
            AcceleratorCount = selection == "accelerator-count" ? 1 : null,
            DiskSizeGb = selection is "disk-size" or "disk-size-type" ? 100 : null,
            DiskType = selection == "disk-size-type" ? GcloudWorkbenchExecutionsCreateDiskType.PdSsd : null,
            NoEnableInternetAccess = selection == "no-internet" ? true : null,
        };
        var errors = new List<ValidationResult>();

        await Assert.That(Validator.TryValidateObject(options, new(options), errors, true)).IsEqualTo(valid)
            .Because(string.Join("; ", errors.Select(error => error.ErrorMessage)));
    }

    [Test]
    public async Task Default_Compute_Settings_Do_Not_Add_Arguments()
    {
        var options = new GcloudWorkbenchExecutionsCreateOptions("region", "execution", "gs://output", "service-account")
        {
            GcsNotebookUri = "gs://notebook.ipynb",
        };

        await AssertArguments(BuildArguments(options),
        [
            "--region=region",
            "--display-name=execution",
            "--gcs-output-uri=gs://output",
            "--service-account=service-account",
            "--gcs-notebook-uri=gs://notebook.ipynb",
        ]);
    }
}
