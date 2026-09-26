using System.ComponentModel.DataAnnotations;
using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    [Arguments("585.0.0")]
    [Arguments("586.0.0")]
    public async Task Gcloud_Kafka_Empty_Mapping_Rules_Count_As_An_Update(string version)
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud", version,
            "gcloud-managed-kafka-clusters-update.txt"));
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("managed-kafka clusters update", help)).Single();
        var group = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains("SslPrincipalMappingRules"));
        var generated = await Generate([.. command.Options.Where(option => group.PropertyNames.Contains(option.PropertyName))],
            alternativeGroups: [group]);
        var optionsType = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(optionsType)!;
        var mappingRules = optionsType.GetProperty("SslPrincipalMappingRules")!;

        foreach (var (value, valid) in new (string?, bool)[]
                 { (null, false), ("", true), ("DEFAULT", true), (" \t", false) })
        {
            mappingRules.SetValue(instance, value);
            var errors = new List<ValidationResult>();
            await Assert.That(Validator.TryValidateObject(instance, new(instance), errors, true)).IsEqualTo(valid)
                .Because($"Mapping rules '{value ?? "<omitted>"}': {string.Join("; ", errors)}");
        }

        mappingRules.SetValue(instance, null);
        optionsType.GetProperty("Cpu")!.SetValue(instance, "");
        await Assert.That(Validator.TryValidateObject(instance, new(instance), [], true)).IsFalse();

        mappingRules.SetValue(instance, "");
        var attribute = mappingRules.GetCustomAttributesData().Single(attribute => attribute.AttributeType.Name == nameof(CliOptionAttribute));
        var optionAttribute = new CliOptionAttribute((string) attribute.ConstructorArguments.Single().Value!)
        {
            Format = (OptionFormat) attribute.NamedArguments.Single(argument => argument.MemberName == nameof(CliOptionAttribute.Format)).TypedValue.Value!,
        };
        var arguments = new CommandArgumentBuilder().BuildArguments(
            [new OptionPart(mappingRules.Name, mappingRules.GetValue, optionAttribute)], instance);
        await Assert.That(arguments).IsEquivalentTo(["--ssl-principal-mapping-rules="]);
    }

    [Test]
    public async Task Gcloud_Sql_Empty_Backup_Location_Remains_Exclusive_With_NoBackup()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "Fixtures", "Gcloud",
            "sql-instances-patch-550.0.0.txt"));
        var command = (await GcloudResourceArgumentTests.ScrapeFixture("sql instances patch", help)).Single();
        var group = command.RequiredAlternativeGroups.Single(group => group.PropertyNames.Contains("BackupLocation"));
        var generated = await Generate([.. command.Options.Where(option => group.PropertyNames.Contains(option.PropertyName))],
            alternativeGroups: [group]);
        var optionsType = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(optionsType)!;
        foreach (var (location, time, valid) in new (string?, string?, bool)[]
                 { (null, null, true), ("", null, true), ("", "03:00", true),
                     (null, "03:00", true), ("europe-west1", "03:00", true) })
        {
            optionsType.GetProperty("BackupLocation")!.SetValue(instance, location);
            optionsType.GetProperty("BackupStartTime")!.SetValue(instance, time);
            await Assert.That(Validator.TryValidateObject(instance, new(instance), [], true)).IsEqualTo(valid)
                .Because($"Backup location '{location ?? "<omitted>"}', time '{time ?? "<omitted>"}'.");
        }

        optionsType.GetProperty("BackupLocation")!.SetValue(instance, "");
        optionsType.GetProperty("BackupStartTime")!.SetValue(instance, null);
        optionsType.GetProperty("NoBackup")!.SetValue(instance, true);
        await Assert.That(Validator.TryValidateObject(instance, new(instance), [], true)).IsFalse();
    }

    [Test]
    public async Task Explicit_Empty_Values_Participate_In_Exclusive_Groups_Without_Allowing_Blank_Operands()
    {
        var generated = await Generate(
            [new() { SwitchName = "--reset", PropertyName = "Reset", CSharpType = "string?", AllowsEmptyValue = true }],
            positionalArguments: [new() { PropertyName = "Resource", CSharpType = "string?", PositionIndex = 0 }],
            alternativeGroups:
            [new()
            {
                IsMutuallyExclusive = true,
                Members =
                [
                    new() { PropertyName = "Reset", OptionSwitch = "--reset" },
                    new() { PropertyName = "Resource", PositionalArgumentPhase = CommandLinePhase.EarlyOperand, PositionalArgumentPositionIndex = 0 },
                ],
            }]);
        var optionsType = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var instance = Activator.CreateInstance(optionsType)!;
        foreach (var (reset, resource, valid) in new (string?, string?, bool)[]
                 { (null, null, false), ("", null, true), ("", "resource", false), (null, "", false),
                     (null, " \t", false), (null, "resource", true), ("rule", null, true), (" \t", null, false) })
        {
            optionsType.GetProperty("Reset")!.SetValue(instance, reset);
            optionsType.GetProperty("Resource")!.SetValue(instance, resource);
            await Assert.That(Validator.TryValidateObject(instance, new(instance), [], true)).IsEqualTo(valid)
                .Because($"Reset '{reset ?? "<omitted>"}', resource '{resource ?? "<omitted>"}'.");
        }
    }
}
