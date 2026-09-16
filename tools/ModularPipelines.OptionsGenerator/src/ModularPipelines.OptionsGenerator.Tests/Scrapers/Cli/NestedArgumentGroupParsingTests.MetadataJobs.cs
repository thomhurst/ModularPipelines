using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class NestedArgumentGroupParsingTests
{
    private static readonly string[] value = new[] { "value" };

    [Test]
    public async Task Captured_Metadata_Job_Validates_Operation_Branches_At_Runtime()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory,
            "Fixtures", "Gcloud", "585.0.0", "gcloud-dataplex-metadata-jobs-create.txt"));
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("dataplex metadata-jobs create", help)).Single();
        var tool = new CliToolDefinition
        {
            ToolName = "gcloud",
            NamespacePrefix = "Gcloud",
            TargetNamespace = "ModularPipelines.Google",
            OutputDirectory = "output",
            Commands = [command],
        };
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        var enums = (await new EnumGenerator().GenerateAsync(tool)).Select(file => file.Content).ToArray();
        await VerifyGeneratedValidation(generated, command.ClassName, async type =>
        {
            var constructor = type.GetConstructors().Single();
            var operationType = constructor.GetParameters().Single().ParameterType;
            foreach (var (properties, valid) in new (string[], bool)[]
            {
                ([], false),
                (["ExportOutputPath"], false),
                (["ExportAspectTypes"], false),
                (["ExportOutputPath", "ExportAspectTypes"], true),
                (["ExportOutputPath", "ExportEntryTypes"], true),
                (["ExportOutputPath", "ExportProjects"], true),
                (["ExportOutputPath", "ExportProjects", "ExportEntryGroups"], false),
                (["ExportOutputPath", "ExportAspectTypes", "ExportEntryTypes"], true),
                (["ExportOutputPath", "ExportProjects", "ExportAspectTypes"], true),
                (["ExportOutputPath", "ExportProjects", "ImportSourceStorageUri"], false),
                (["ImportSourceStorageUri"], false),
                (["ImportAspectSyncMode", "ImportEntrySyncMode", "ImportSourceStorageUri"], false),
                (["ImportAspectSyncMode", "ImportEntrySyncMode", "ImportSourceStorageUri", "ImportEntryGroups"], true),
                (["ImportAspectSyncMode", "ImportEntrySyncMode", "ImportSourceStorageUri", "ImportEntryGroups", "ImportAspectTypes"], true),
            })
            {
                var operation = properties.Any(name => name.StartsWith("Export", StringComparison.Ordinal)) ? "EXPORT" : "IMPORT";
                var instance = constructor.Invoke([operationType == typeof(string)
                    ? operation : Enum.Parse(operationType, operation, ignoreCase: true)]);
                foreach (var name in properties)
                {
                    var property = type.GetProperty(name)!;
                    property.SetValue(instance, property.PropertyType == typeof(string)
                        ? name.EndsWith("SyncMode", StringComparison.Ordinal) ? "FULL" : "value"
                        : value);
                }
                var errors = ((IValidatableObject) instance).Validate(new(instance)).ToArray();
                await Assert.That(errors.Length == 0).IsEqualTo(valid)
                    .Because(string.Join(", ", properties) + ": " + string.Join("; ", errors.Select(error => error.ErrorMessage)));
            }
        }, enums);
    }
}
