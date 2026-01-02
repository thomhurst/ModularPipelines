using System.Text;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class GeneratorUtilsTests
{
    #region ToPascalCase Tests

    [Test]
    public async Task ToPascalCase_Converts_Kebab_Case_Correctly()
    {
        var result = GeneratorUtils.ToPascalCase("dry-run");

        await Assert.That(result).IsEqualTo("DryRun");
    }

    [Test]
    public async Task ToPascalCase_Converts_Snake_Case_Correctly()
    {
        var result = GeneratorUtils.ToPascalCase("dry_run");

        await Assert.That(result).IsEqualTo("DryRun");
    }

    [Test]
    public async Task ToPascalCase_Handles_Multiple_Separators()
    {
        var result = GeneratorUtils.ToPascalCase("my-test_value");

        await Assert.That(result).IsEqualTo("MyTestValue");
    }

    [Test]
    public async Task ToPascalCase_Handles_Single_Word()
    {
        var result = GeneratorUtils.ToPascalCase("verbose");

        await Assert.That(result).IsEqualTo("Verbose");
    }

    [Test]
    public async Task ToPascalCase_Returns_Empty_For_Empty_Input()
    {
        var result = GeneratorUtils.ToPascalCase("");

        await Assert.That(result).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task ToPascalCase_Returns_Empty_For_Whitespace_Input()
    {
        var result = GeneratorUtils.ToPascalCase("   ");

        await Assert.That(result).IsEqualTo(string.Empty);
    }

    #endregion

    #region EscapeXmlComment Tests

    [Test]
    public async Task EscapeXmlComment_Escapes_Ampersand()
    {
        var result = GeneratorUtils.EscapeXmlComment("foo & bar");

        await Assert.That(result).IsEqualTo("foo &amp; bar");
    }

    [Test]
    public async Task EscapeXmlComment_Escapes_Less_Than()
    {
        var result = GeneratorUtils.EscapeXmlComment("value < 10");

        await Assert.That(result).IsEqualTo("value &lt; 10");
    }

    [Test]
    public async Task EscapeXmlComment_Escapes_Greater_Than()
    {
        var result = GeneratorUtils.EscapeXmlComment("value > 10");

        await Assert.That(result).IsEqualTo("value &gt; 10");
    }

    [Test]
    public async Task EscapeXmlComment_Replaces_Newlines_With_Spaces()
    {
        var result = GeneratorUtils.EscapeXmlComment("line1\nline2\r\nline3");

        await Assert.That(result).IsEqualTo("line1 line2 line3");
    }

    [Test]
    public async Task EscapeXmlComment_Returns_Empty_For_Null_Input()
    {
        var result = GeneratorUtils.EscapeXmlComment(null);

        await Assert.That(result).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task EscapeXmlComment_Returns_Empty_For_Empty_Input()
    {
        var result = GeneratorUtils.EscapeXmlComment("");

        await Assert.That(result).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task EscapeXmlComment_Trims_Whitespace()
    {
        var result = GeneratorUtils.EscapeXmlComment("  test  ");

        await Assert.That(result).IsEqualTo("test");
    }

    #endregion

    #region EscapeIdentifier Tests

    [Test]
    public async Task EscapeIdentifier_Escapes_Reserved_Keyword()
    {
        var result = GeneratorUtils.EscapeIdentifier("class");

        await Assert.That(result).IsEqualTo("@class");
    }

    [Test]
    public async Task EscapeIdentifier_Does_Not_Escape_Non_Keyword()
    {
        var result = GeneratorUtils.EscapeIdentifier("MyClass");

        await Assert.That(result).IsEqualTo("MyClass");
    }

    [Test]
    [Arguments("abstract")]
    [Arguments("namespace")]
    [Arguments("string")]
    [Arguments("int")]
    [Arguments("public")]
    [Arguments("static")]
    public async Task EscapeIdentifier_Escapes_Various_Keywords(string keyword)
    {
        var result = GeneratorUtils.EscapeIdentifier(keyword);

        await Assert.That(result).IsEqualTo($"@{keyword}");
    }

    [Test]
    public async Task EscapeIdentifier_Returns_Empty_For_Null()
    {
        var result = GeneratorUtils.EscapeIdentifier(null);

        await Assert.That(result).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task EscapeIdentifier_Returns_Empty_For_Empty_Input()
    {
        var result = GeneratorUtils.EscapeIdentifier("");

        await Assert.That(result).IsEqualTo(string.Empty);
    }

    #endregion

    #region ToEnumMemberName Tests

    [Test]
    public async Task ToEnumMemberName_Converts_Kebab_Case()
    {
        var result = GeneratorUtils.ToEnumMemberName("dry-run");

        await Assert.That(result).IsEqualTo("DryRun");
    }

    [Test]
    public async Task ToEnumMemberName_Prefixes_Numeric_Values()
    {
        var result = GeneratorUtils.ToEnumMemberName("1");

        await Assert.That(result).IsEqualTo("Value1");
    }

    [Test]
    public async Task ToEnumMemberName_Returns_Unknown_For_Empty()
    {
        var result = GeneratorUtils.ToEnumMemberName("");

        await Assert.That(result).IsEqualTo("Unknown");
    }

    [Test]
    public async Task ToEnumMemberName_Returns_Unknown_For_Whitespace()
    {
        var result = GeneratorUtils.ToEnumMemberName("   ");

        await Assert.That(result).IsEqualTo("Unknown");
    }

    #endregion

    #region ToEnumName Tests

    [Test]
    public async Task ToEnumName_Combines_Prefix_And_PascalCase_Name()
    {
        var result = GeneratorUtils.ToEnumName("--verbosity", "Build");

        await Assert.That(result).IsEqualTo("BuildVerbosity");
    }

    [Test]
    public async Task ToEnumName_Strips_Leading_Dashes()
    {
        var result = GeneratorUtils.ToEnumName("---test-option", "Docker");

        await Assert.That(result).IsEqualTo("DockerTestOption");
    }

    #endregion

    #region GenerateCliAttributeString Tests

    [Test]
    public async Task GenerateCliAttributeString_Returns_CliFlag_For_Boolean_Flag()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--verbose",
            PropertyName = "Verbose",
            CSharpType = "bool?",
            IsFlag = true,
        };

        var result = GeneratorUtils.GenerateCliAttributeString(option);

        await Assert.That(result).IsEqualTo("CliFlag(\"--verbose\")");
    }

    [Test]
    public async Task GenerateCliAttributeString_Returns_CliFlag_With_ShortForm()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--verbose",
            ShortForm = "-v",
            PropertyName = "Verbose",
            CSharpType = "bool?",
            IsFlag = true,
        };

        var result = GeneratorUtils.GenerateCliAttributeString(option);

        await Assert.That(result).IsEqualTo("CliFlag(\"--verbose\", ShortForm = \"-v\")");
    }

    [Test]
    public async Task GenerateCliAttributeString_Returns_CliOption_For_Value_Option()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--output",
            PropertyName = "Output",
            CSharpType = "string?",
            IsFlag = false,
        };

        var result = GeneratorUtils.GenerateCliAttributeString(option);

        await Assert.That(result).IsEqualTo("CliOption(\"--output\")");
    }

    [Test]
    public async Task GenerateCliAttributeString_Includes_EqualsSeparator_Format()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--output",
            PropertyName = "Output",
            CSharpType = "string?",
            IsFlag = false,
            ValueSeparator = "=",
        };

        var result = GeneratorUtils.GenerateCliAttributeString(option);

        await Assert.That(result).Contains("Format = OptionFormat.EqualsSeparated");
    }

    [Test]
    public async Task GenerateCliAttributeString_Includes_ColonSeparator_Format()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--output",
            PropertyName = "Output",
            CSharpType = "string?",
            IsFlag = false,
            ValueSeparator = ":",
        };

        var result = GeneratorUtils.GenerateCliAttributeString(option);

        await Assert.That(result).Contains("Format = OptionFormat.ColonSeparated");
    }

    [Test]
    public async Task GenerateCliAttributeString_Includes_AllowMultiple_When_True()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--values",
            PropertyName = "Values",
            CSharpType = "string[]?",
            IsFlag = false,
            AcceptsMultipleValues = true,
        };

        var result = GeneratorUtils.GenerateCliAttributeString(option);

        await Assert.That(result).Contains("AllowMultiple = true");
    }

    #endregion

    #region GenerateMethodNameFromCommandParts Tests

    [Test]
    public async Task GenerateMethodNameFromCommandParts_Converts_To_PascalCase()
    {
        var result = GeneratorUtils.GenerateMethodNameFromCommandParts(["container", "create"]);

        await Assert.That(result).IsEqualTo("ContainerCreate");
    }

    [Test]
    public async Task GenerateMethodNameFromCommandParts_Handles_Kebab_Case_Parts()
    {
        var result = GeneratorUtils.GenerateMethodNameFromCommandParts(["build-server"]);

        await Assert.That(result).IsEqualTo("BuildServer");
    }

    [Test]
    public async Task GenerateMethodNameFromCommandParts_Handles_Empty_Array()
    {
        var result = GeneratorUtils.GenerateMethodNameFromCommandParts([]);

        await Assert.That(result).IsEqualTo(string.Empty);
    }

    #endregion

    #region GenerateFileHeader Tests

    [Test]
    public async Task GenerateFileHeader_Includes_Auto_Generated_Comment()
    {
        var sb = new StringBuilder();

        GeneratorUtils.GenerateFileHeader(sb);

        await Assert.That(sb.ToString()).Contains("// <auto-generated>");
        await Assert.That(sb.ToString()).Contains("// </auto-generated>");
    }

    [Test]
    public async Task GenerateFileHeader_Includes_Source_Url_When_Provided()
    {
        var sb = new StringBuilder();

        GeneratorUtils.GenerateFileHeader(sb, "https://example.com/docs");

        await Assert.That(sb.ToString()).Contains("// Source: https://example.com/docs");
    }

    [Test]
    public async Task GenerateFileHeaderWithNullable_Includes_Nullable_Enable()
    {
        var sb = new StringBuilder();

        GeneratorUtils.GenerateFileHeaderWithNullable(sb);

        await Assert.That(sb.ToString()).Contains("#nullable enable");
    }

    #endregion

    #region GenerateXmlDocumentation Tests

    [Test]
    public async Task GenerateXmlDocumentation_Generates_Summary_Block()
    {
        var sb = new StringBuilder();

        GeneratorUtils.GenerateXmlDocumentation(sb, "Test description");

        var result = sb.ToString();
        await Assert.That(result).Contains("/// <summary>");
        await Assert.That(result).Contains("/// Test description");
        await Assert.That(result).Contains("/// </summary>");
    }

    [Test]
    public async Task GenerateXmlDocumentation_Does_Nothing_For_Empty_Description()
    {
        var sb = new StringBuilder();

        GeneratorUtils.GenerateXmlDocumentation(sb, "");

        await Assert.That(sb.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task GenerateXmlDocumentation_Does_Nothing_For_Null_Description()
    {
        var sb = new StringBuilder();

        GeneratorUtils.GenerateXmlDocumentation(sb, null);

        await Assert.That(sb.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task GenerateXmlDocumentation_Uses_Custom_Indent()
    {
        var sb = new StringBuilder();

        GeneratorUtils.GenerateXmlDocumentation(sb, "Test", "        ");

        await Assert.That(sb.ToString()).Contains("        /// <summary>");
    }

    #endregion

    #region GenerateValidationAttributes Tests

    [Test]
    public async Task GenerateValidationAttributes_Generates_Range_Attribute()
    {
        var sb = new StringBuilder();
        var constraints = new CliValidationConstraints
        {
            MinValue = 1,
            MaxValue = 100,
        };

        GeneratorUtils.GenerateValidationAttributes(sb, constraints);

        await Assert.That(sb.ToString()).Contains("[Range(1, 100)]");
    }

    [Test]
    public async Task GenerateValidationAttributes_Generates_Regex_Attribute()
    {
        var sb = new StringBuilder();
        var constraints = new CliValidationConstraints
        {
            Pattern = "^[a-z]+$",
        };

        GeneratorUtils.GenerateValidationAttributes(sb, constraints);

        await Assert.That(sb.ToString()).Contains("[RegularExpression(@\"^[a-z]+$\")]");
    }

    [Test]
    public async Task GenerateValidationAttributes_Handles_Only_MinValue()
    {
        var sb = new StringBuilder();
        var constraints = new CliValidationConstraints
        {
            MinValue = 5,
        };

        GeneratorUtils.GenerateValidationAttributes(sb, constraints);

        await Assert.That(sb.ToString()).Contains("[Range(5,");
    }

    [Test]
    public async Task GenerateValidationAttributes_Uses_Custom_Indent()
    {
        var sb = new StringBuilder();
        var constraints = new CliValidationConstraints
        {
            MinValue = 1,
            MaxValue = 10,
        };

        GeneratorUtils.GenerateValidationAttributes(sb, constraints, "        ");

        await Assert.That(sb.ToString()).StartsWith("        [Range");
    }

    #endregion
}
