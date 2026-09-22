using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Generators;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class NestedArgumentGroupParsingTests
{
    [Test]
    public async Task Captured_Workbench_Execution_Compute_Settings_Remain_Optional()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory,
            "Fixtures", "Gcloud", "586.0.0", "gcloud-workbench-executions-create.txt"));
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("workbench executions create", help)).Single();
        await Assert.That(command.RequiredOptions.Select(option => option.PropertyName))
            .IsEquivalentTo(["Region", "DisplayName", "GcsOutputUri", "ServiceAccount"]);

        var tool = CreateGcloudValidationTool(command);
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        var enums = (await new EnumGenerator().GenerateAsync(tool)).Select(file => file.Content).ToArray();
        await VerifyGeneratedValidation(generated, command.ClassName, async type =>
        {
            var accelerator = command.Options.Single(option => option.PropertyName == "AcceleratorType").EnumDefinition!.Values[0].MemberName;
            var disk = command.Options.Single(option => option.PropertyName == "DiskType").EnumDefinition!.Values[0].MemberName;
            (string Name, bool Valid, (string Property, object? Value)[] Settings)[] cases =
            [
                ("default", true, []),
                ("machine", true, [("MachineType", "e2-standard-4")]),
                ("accelerator", false, [("AcceleratorType", accelerator)]),
                ("accelerator-count", true, [("AcceleratorType", accelerator), ("AcceleratorCount", 1)]),
                ("disk-size", false, [("DiskSizeGb", 100)]),
                ("disk-size-type", true, [("DiskSizeGb", 100), ("DiskType", disk)]),
                ("no-internet", true, [("NoEnableInternetAccess", true)]),
                ("no-source", false, [("GcsNotebookUri", null)]),
            ];
            foreach (var (selection, valid, settings) in cases)
            {
                var instance = Activator.CreateInstance(type, "region", "execution", "gs://output", "service-account")!;
                type.GetProperty("GcsNotebookUri")!.SetValue(instance, "gs://notebook.ipynb");
                foreach (var (name, value) in settings)
                {
                    var property = type.GetProperty(name)!;
                    var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    property.SetValue(instance, propertyType.IsEnum && value is string text ? Enum.Parse(propertyType, text) : value);
                }

                var errors = ((IValidatableObject) instance).Validate(new(instance)).ToArray();
                await Assert.That(errors.Length == 0).IsEqualTo(valid)
                    .Because(selection + ": " + string.Join("; ", errors.Select(error => error.ErrorMessage)));
            }
        }, enums);
    }

    [Test]
    [Arguments("oracle-database goldengate-deployments create", "MaintenanceConfig")]
    [Arguments("oracle-database goldengate-deployments create", "MaintenanceWindow")]
    [Arguments("oracle-database exadb-vm-clusters create", "TimeZone")]
    public async Task Captured_Optional_Settings_Do_Not_Inherit_Required_Section(string path, string prefix)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory,
            "Fixtures", "Gcloud", "586.0.0", $"gcloud-{path.Replace(' ', '-')}.txt"));
        var command = (await GcloudResourceArgumentTests.ScrapeFixture(path, help)).Single();
        var settings = command.Options.Where(option => option.PropertyName.StartsWith(prefix, StringComparison.Ordinal)).ToArray();
        await Assert.That(settings).IsNotEmpty();
        await Assert.That(settings.Where(option => option.IsRequired).Select(option => option.PropertyName)).IsEmpty();
    }
}
