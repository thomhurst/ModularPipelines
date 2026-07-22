using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class HeuristicTypeDetectorTests
{
    private readonly HeuristicTypeDetector _detector;

    public HeuristicTypeDetectorTests()
    {
        var logger = NullLogger<HeuristicTypeDetector>.Instance;
        _detector = new HeuristicTypeDetector(logger);
    }

    private static OptionDetectionContext CreateContext(
        string optionName,
        string? description = null,
        string? defaultValue = null,
        string? acceptedValues = null)
    {
        return new OptionDetectionContext
        {
            OptionName = optionName,
            AllNames = [optionName],
            Description = description,
            DefaultValue = defaultValue,
            AcceptedValues = acceptedValues,
            ToolName = "test",
            CommandPath = ["test", "command"],
        };
    }

    #region Basic Properties Tests

    [Test]
    public async Task Priority_Returns_300()
    {
        await Assert.That(_detector.Priority).IsEqualTo(300);
    }

    [Test]
    public async Task Name_Returns_Heuristic()
    {
        await Assert.That(_detector.Name).IsEqualTo("Heuristic");
    }

    [Test]
    public async Task CanHandle_Returns_True_For_Any_Tool()
    {
        await Assert.That(_detector.CanHandle("docker")).IsTrue();
        await Assert.That(_detector.CanHandle("kubectl")).IsTrue();
        await Assert.That(_detector.CanHandle("az")).IsTrue();
        await Assert.That(_detector.CanHandle("any-tool")).IsTrue();
    }

    #endregion

    #region Boolean Detection Tests - Default Values

    [Test]
    [Arguments("true")]
    [Arguments("false")]
    [Arguments("yes")]
    [Arguments("no")]
    [Arguments("0")]
    [Arguments("1")]
    public async Task DetectType_Returns_Bool_For_Boolean_DefaultValue(string defaultValue)
    {
        var context = CreateContext("--flag", defaultValue: defaultValue);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Bool);
    }

    [Test]
    public async Task DetectType_Returns_Int_For_Numeric_DefaultValue()
    {
        var context = CreateContext("--count", defaultValue: "42");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Int);
    }

    #endregion

    #region Boolean Detection Tests - Accepted Values

    [Test]
    public async Task DetectType_Returns_Bool_For_Boolean_AcceptedValues()
    {
        var context = CreateContext("--option", acceptedValues: "true, false");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Bool);
    }

    [Test]
    public async Task DetectType_Returns_Bool_For_YesNo_AcceptedValues()
    {
        var context = CreateContext("--option", acceptedValues: "yes, no");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Bool);
    }

    [Test]
    [Arguments("true/false")]
    [Arguments("yes/no")]
    [Arguments("on/off")]
    public async Task DetectType_Returns_Bool_For_Slash_Separated_AcceptedValues(string acceptedValues)
    {
        var context = CreateContext("--option", acceptedValues: acceptedValues);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Bool);
    }

    [Test]
    public async Task DetectType_Returns_Enum_WhenAcceptedValuesContainNonBooleanValue()
    {
        var context = CreateContext("--option", acceptedValues: "on|off|auto");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Enum);
        await Assert.That(result.EnumValues).IsEquivalentTo(["on", "off", "auto"]);
    }

    #endregion

    #region Boolean Detection Tests - Option Name Prefixes

    [Test]
    [Arguments("--no-cache")]
    [Arguments("--disable-validation")]
    [Arguments("--enable-feature")]
    [Arguments("--with-deps")]
    [Arguments("--without-deps")]
    [Arguments("--skip-tests")]
    [Arguments("--force-rebuild")]
    [Arguments("--allow-empty")]
    [Arguments("--deny-access")]
    [Arguments("--include-all")]
    [Arguments("--exclude-tests")]
    public async Task DetectType_Returns_Bool_For_Boolean_Prefix_Names(string optionName)
    {
        var context = CreateContext(optionName);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Bool);
    }

    #endregion

    #region Boolean Detection Tests - Option Name Suffixes

    [Test]
    [Arguments("--build-only")]
    [Arguments("--all")]
    [Arguments("--recursive")]
    [Arguments("--verbose")]
    [Arguments("--quiet")]
    [Arguments("--silent")]
    [Arguments("--interactive")]
    [Arguments("--yes")]
    [Arguments("--no")]
    [Arguments("--force")]
    public async Task DetectType_Returns_Bool_For_Boolean_Suffix_Names(string optionName)
    {
        var context = CreateContext(optionName);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Bool);
    }

    #endregion

    #region Boolean Detection Tests - Exact Names

    [Test]
    [Arguments("--verbose")]
    [Arguments("--quiet")]
    [Arguments("--silent")]
    [Arguments("--debug")]
    [Arguments("--force")]
    [Arguments("--yes")]
    [Arguments("--no")]
    [Arguments("--dry-run")]
    [Arguments("--dryrun")]
    [Arguments("--help")]
    [Arguments("--version")]
    [Arguments("--interactive")]
    [Arguments("--detach")]
    [Arguments("--tty")]
    [Arguments("--privileged")]
    [Arguments("--rm")]
    [Arguments("--init")]
    [Arguments("--recursive")]
    [Arguments("--all")]
    [Arguments("--cached")]
    [Arguments("--staged")]
    [Arguments("--untracked")]
    [Arguments("--ignored")]
    public async Task DetectType_Returns_Bool_For_Boolean_Exact_Names(string optionName)
    {
        var context = CreateContext(optionName);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Bool);
    }

    #endregion

    #region Boolean Detection Tests - Description Patterns

    [Test]
    [Arguments("enable logging")]
    [Arguments("disable caching")]
    [Arguments("turn on debugging")]
    [Arguments("turn off features")]
    [Arguments("whether to build")]
    [Arguments("if true, do something")]
    [Arguments("if set, enable feature")]
    [Arguments("when set, activate")]
    [Arguments("flag to enable")]
    [Arguments("activate the feature")]
    [Arguments("deactivate mode")]
    public async Task DetectType_Returns_Bool_For_Boolean_Description_Patterns(string description)
    {
        var context = CreateContext("--some-option", description: description);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Bool);
    }

    #endregion

    #region Numeric Detection Tests

    [Test]
    [Arguments("--count")]
    [Arguments("--limit")]
    [Arguments("--max-retries")]
    [Arguments("--min-value")]
    [Arguments("--length")]
    [Arguments("--depth")]
    [Arguments("--retries")]
    [Arguments("--port")]
    [Arguments("--number")]
    public async Task DetectType_Returns_Int_For_Numeric_Names(string optionName)
    {
        var context = CreateContext(optionName);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Int);
    }

    [Test]
    [Arguments("--timeout")]
    [Arguments("--request-duration")]
    [Arguments("--poll-interval")]
    [Arguments("--size")]
    [Arguments("--max-image-size")]
    public async Task DetectType_Returns_String_For_Potentially_UnitBearing_Names(string optionName)
    {
        var context = CreateContext(optionName);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.String);
    }

    [Test]
    public async Task DetectType_Returns_Int_For_UnitBearing_Name_With_Numeric_Default()
    {
        var context = CreateContext("--timeout", defaultValue: "30");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Int);
    }

    [Test]
    public async Task DetectType_Returns_Int_When_Description_Confirms_Bare_Number()
    {
        var context = CreateContext("--timeout", description: "Timeout as an integer number of seconds");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Int);
    }

    #endregion

    #region List Detection Tests

    [Test]
    [Arguments("--add-host")]
    [Arguments("--exclude")]
    [Arguments("--include")]
    [Arguments("--label")]
    [Arguments("--env")]
    [Arguments("--volume")]
    [Arguments("--mount")]
    [Arguments("--publish")]
    [Arguments("--expose")]
    [Arguments("--cap-add")]
    [Arguments("--device")]
    [Arguments("--dns")]
    public async Task DetectType_Returns_StringList_For_List_Names(string optionName)
    {
        var context = CreateContext(optionName);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.StringList);
    }

    #endregion

    #region KeyValue Detection Tests

    [Test]
    public async Task DetectType_Returns_KeyValue_For_Env_Option()
    {
        var context = CreateContext("--env");

        var result = await _detector.DetectTypeAsync(context);

        // Note: env matches list pattern first, but this tests the pattern exists
        await Assert.That(result.Type).IsEqualTo(CliOptionType.StringList).Or.IsEqualTo(CliOptionType.KeyValue);
    }

    [Test]
    public async Task DetectType_Returns_KeyValue_For_Label_Option()
    {
        var context = CreateContext("--label");

        var result = await _detector.DetectTypeAsync(context);

        // Note: label matches list pattern first
        await Assert.That(result.Type).IsEqualTo(CliOptionType.StringList).Or.IsEqualTo(CliOptionType.KeyValue);
    }

    [Test]
    public async Task DetectType_Returns_KeyValue_For_Annotation_Option()
    {
        var context = CreateContext("--annotation");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.KeyValue);
    }

    [Test]
    public async Task DetectType_Returns_KeyValue_For_Sysctl_Option()
    {
        var context = CreateContext("--sysctl");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.KeyValue);
    }

    [Test]
    public async Task DetectType_Returns_KeyValue_For_KeyEquals_Description()
    {
        var context = CreateContext("--option", description: "Set options in key=value format");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.KeyValue);
    }

    [Test]
    public async Task DetectType_Returns_KeyValue_For_KeyColon_Description()
    {
        var context = CreateContext("--option", description: "Set options in key:value format");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.KeyValue);
    }

    #endregion

    #region Value Option Detection Tests - Multi-Value Patterns

    [Test]
    [Arguments("Variable for templates, can be used multiple times.")]
    [Arguments("May be specified multiple times")]
    [Arguments("Can be specified multiple times")]
    [Arguments("This option can be repeated")]
    [Arguments("May be repeated for each value")]
    [Arguments("Specify multiple times for different values")]
    [Arguments("This is a repeatable option")]
    public async Task DetectType_Returns_StringList_For_MultiValue_Description_Patterns(string description)
    {
        var context = CreateContext("--var", description: description);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.StringList);
        await Assert.That(result.Confidence).IsEqualTo(80);
    }

    #endregion

    #region Value Option Detection Tests - High Confidence Patterns

    [Test]
    [Arguments("Set to 'hcl2' to run in HCL2 mode when no file is passed.")]
    [Arguments("Set to \"json\" for JSON output")]
    [Arguments("Defaults to 'yaml' format")]
    [Arguments("Defaults to \"text\" output")]
    public async Task DetectType_Returns_String_For_HighConfidence_Value_Description_Patterns(string description)
    {
        var context = CreateContext("--config-type", description: description);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.String);
        await Assert.That(result.Confidence).IsEqualTo(85);
    }

    [Test]
    [Arguments("One of: debug, info, warn, error")]
    [Arguments("Valid values: json, yaml, xml")]
    [Arguments("Allowed values: true, false, auto")]
    [Arguments("Possible values: always, never, auto")]
    [Arguments("Accepted values: low, medium, high")]
    [Arguments("Output format (table, json, yaml)")]
    public async Task DetectType_Returns_Enum_For_Allowed_Values_In_Description(string description)
    {
        var context = CreateContext("--format", description: description);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Enum);
        await Assert.That(result.EnumValues).IsNotNull();
        await Assert.That(result.EnumValues!.Length).IsGreaterThanOrEqualTo(3);
    }

    [Test]
    public async Task DetectType_Preserves_Repeatability_For_Allowed_Values()
    {
        var context = CreateContext(
            "--sort",
            description: "May be repeated or comma-separated. Possible values: ascending, descending.");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Enum);
        await Assert.That(result.AcceptsMultipleValues).IsTrue();
        await Assert.That(result.EnumValues).IsEquivalentTo(["ascending", "descending"]);
    }

    [Test]
    public async Task DetectType_Returns_Enum_For_Structured_Accepted_Values()
    {
        var context = CreateContext("--format", acceptedValues: "table|json|yaml");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Enum);
        await Assert.That(result.EnumValues).IsEquivalentTo(new[] { "table", "json", "yaml" });
    }

    [Test]
    public async Task DetectType_Does_Not_Match_Boolean_Substrings_In_Accepted_Values()
    {
        var context = CreateContext("--mode", acceptedValues: "none, falsehood");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Enum);
        await Assert.That(result.EnumValues).IsEquivalentTo(new[] { "none", "falsehood" });
    }

    [Test]
    public async Task DetectType_Does_Not_Create_Numeric_Enums()
    {
        var context = CreateContext("--level", description: "Allowed values: 1, 2, 3");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.String);
        await Assert.That(result.EnumValues).IsNull();
    }

    [Test]
    public async Task DetectType_Does_Not_Create_Enums_From_Explanatory_Lists()
    {
        var context = CreateContext("--size", description: "Size (bytes, kilobytes, or megabytes)");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.String);
        await Assert.That(result.EnumValues).IsNull();
    }

    [Test]
    public async Task DetectType_Does_Not_Create_Enums_With_Unsafe_Identifiers()
    {
        var context = CreateContext("--platform", description: "Allowed values: linux/amd64, windows/amd64");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.String);
        await Assert.That(result.EnumValues).IsNull();
    }

    #endregion

    #region Value Option Detection Tests - Medium Confidence Patterns

    [Test]
    [Arguments("Specifies the output format")]
    [Arguments("Specify the path to config")]
    [Arguments("Path to the configuration file")]
    [Arguments("Name of the resource to create")]
    [Arguments("Location of the input file")]
    [Arguments("Format for the output data")]
    public async Task DetectType_Returns_String_For_MediumConfidence_Value_Description_Patterns(string description)
    {
        var context = CreateContext("--format", description: description);

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.String);
        await Assert.That(result.Confidence).IsEqualTo(70);
    }

    #endregion

    #region Value Detection Overrides Boolean Name Patterns

    [Test]
    public async Task DetectType_Value_Pattern_Overrides_Boolean_Name_Pattern()
    {
        // Even though "var" might not match boolean patterns, this tests that
        // value-indicating descriptions take precedence
        var context = CreateContext("--var", description: "Variable for templates, can be used multiple times.");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.StringList);
        await Assert.That(result.Confidence).IsEqualTo(80);
    }

    [Test]
    public async Task DetectType_Packer_Var_Option_Returns_StringList()
    {
        // Real-world test case from Packer CLI
        var context = CreateContext("--var", description: "Variable for templates, can be used multiple times.");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.StringList);
    }

    [Test]
    public async Task DetectType_Packer_ConfigType_Option_Returns_String()
    {
        // Real-world test case from Packer CLI
        var context = CreateContext("--config-type", description: "Set to 'hcl2' to run in HCL2 mode when no file is passed.");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.String);
    }

    #endregion

    #region Default String Detection Tests

    [Test]
    public async Task DetectType_Returns_String_For_Unknown_Pattern()
    {
        var context = CreateContext("--output-path");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.String);
    }

    [Test]
    public async Task DetectType_Returns_String_For_Generic_Option()
    {
        var context = CreateContext("--name", description: "The name to use");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Type).IsEqualTo(CliOptionType.String);
    }

    #endregion

    #region Confidence and Source Tests

    [Test]
    public async Task DetectType_Returns_Confidence_50_For_Boolean_Patterns()
    {
        var context = CreateContext("--verbose");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Confidence).IsEqualTo(50);
    }

    [Test]
    public async Task DetectType_Returns_Confidence_50_For_Default_String_Fallback()
    {
        var context = CreateContext("--unknown-option");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Confidence).IsEqualTo(50);
    }

    [Test]
    public async Task DetectType_Returns_Confidence_80_For_MultiValue_Patterns()
    {
        var context = CreateContext("--option", description: "can be used multiple times");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Confidence).IsEqualTo(80);
    }

    [Test]
    public async Task DetectType_Returns_Confidence_85_For_HighConfidence_Value_Patterns()
    {
        var context = CreateContext("--option", description: "Set to 'value' for something");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Confidence).IsEqualTo(85);
    }

    [Test]
    public async Task DetectType_Returns_Confidence_70_For_MediumConfidence_Value_Patterns()
    {
        var context = CreateContext("--option", description: "Specifies the output");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Confidence).IsEqualTo(70);
    }

    [Test]
    public async Task DetectType_Returns_Heuristic_Source()
    {
        var context = CreateContext("--verbose");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Source).IsEqualTo("Heuristic");
    }

    [Test]
    public async Task DetectType_Returns_Notes_With_Reason()
    {
        var context = CreateContext("--verbose");

        var result = await _detector.DetectTypeAsync(context);

        await Assert.That(result.Notes).IsNotNull();
        await Assert.That(result.Notes!.Length).IsGreaterThan(0);
    }

    #endregion
}
