using System.Text;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Generators;
using ModularPipelines.OptionsGenerator.Models;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public class GeneratorUtilsTests
{
    [Test]
    public async Task GenerateAssemblyInfo_Uses_Stable_Package_Metadata()
    {
        var result = GeneratorUtils.GenerateAssemblyInfo("ModularPipelines.Java");

        await Assert.That(result).Contains(
            "AssemblyMetadata(\"ModularPipelines.OptionsGenerator.Package\", \"ModularPipelines.Java\")");
        await Assert.That(result).DoesNotContain("ModularPipelines.OptionsGenerator.Tool");
    }

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
    [Arguments("BuildServer", "BuildServer")]
    [Arguments("buildserver", "BuildServer")]
    [Arguments("appconfig", "AppConfig")]
    [Arguments("disk-encryption-set", "DiskEncryptionSet")]
    [Arguments("resourcemanagement", "ResourceManagement")]
    [Arguments("restorepoint", "RestorePoint")]
    [Arguments("resourcemanager", "ResourceManager")]
    [Arguments("imagetools", "ImageTools")]
    [Arguments("binarylogger", "BinaryLogger")]
    [Arguments("nologo", "NoLogo")]
    [Arguments("nuget", "NuGet")]
    [Arguments("agenttask", "AgentTask")]
    [Arguments("clusterinfo", "ClusterInfo")]
    [Arguments("gpgkey", "GpgKey")]
    [Arguments("sshkey", "SshKey")]
    [Arguments("kubeconfig", "KubeConfig")]
    [Arguments("9p", "_9p")]
    public async Task ToPascalCase_Handles_Compound_Words(
        string input,
        string expected)
    {
        var result = GeneratorUtils.ToPascalCase(input);

        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments("ChartRevision", "ChartRevision")]
    [Arguments("IfNotPresent", "IfNotPresent")]
    [Arguments("ConfigMap", "ConfigMap")]
    [Arguments("OCIRepository", "OciRepository")]
    public async Task ToEnumMemberName_Preserves_Existing_Word_Boundaries(
        string input,
        string expected)
    {
        var result = GeneratorUtils.ToEnumMemberName(input);

        await Assert.That(result).IsEqualTo(expected);
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
    public async Task EscapeXmlComment_Replaces_All_Control_Characters_With_Spaces()
    {
        foreach (var controlCharacter in Enumerable.Range(char.MinValue, char.MaxValue + 1)
                     .Select(static value => (char) value)
                     .Where(char.IsControl))
        {
            var result = GeneratorUtils.EscapeXmlComment($"before{controlCharacter}after");

            await Assert.That(result).IsEqualTo("before after");
        }
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
    public async Task ToEnumMemberName_Removes_Identifier_Punctuation()
    {
        var result = GeneratorUtils.ToEnumMemberName("cyclonedx1.4+json");

        await Assert.That(result).IsEqualTo("Cyclonedx14Json");
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
    public async Task GenerateCliAttributeString_Returns_CliFlag_With_Negated_Name()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--feature",
            NegatedSwitchName = "--no-feature",
            PropertyName = "Feature",
            CSharpType = "bool?",
            IsFlag = true,
        };

        var result = GeneratorUtils.GenerateCliAttributeString(option);

        await Assert.That(result)
            .IsEqualTo("CliFlag(\"--feature\", NegatedName = \"--no-feature\")");
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
    public async Task GenerateCliAttributeString_Includes_NoSeparator_Format()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "-D",
            PropertyName = "Property",
            CSharpType = "IReadOnlyList<KeyValue>?",
            IsFlag = false,
            ValueSeparator = string.Empty,
        };

        var result = GeneratorUtils.GenerateCliAttributeString(option);

        await Assert.That(result).Contains("Format = OptionFormat.NoSeparator");
    }

    [Test]
    public async Task GenerateCliAttributeString_Includes_OptionalArity_And_TerminalPhase()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--run-tests",
            PropertyName = "RunTests",
            CSharpType = "string?",
            ValueArity = CliOptionValueArity.Optional,
            Phase = CommandLinePhase.Terminal,
        };

        var result = GeneratorUtils.GenerateCliAttributeString(option);

        await Assert.That(result).IsEqualTo(
            "CliOption(\"--run-tests\", ValueArity = CliOptionValueArity.Optional, " +
            "Phase = CommandLinePhase.Terminal)");
    }

    [Test]
    public async Task GenerateCliAttributeString_Includes_Grouped_Values()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--arguments",
            PropertyName = "Arguments",
            CSharpType = "string[]?",
            GroupValues = true,
        };

        var attribute = GeneratorUtils.GenerateCliAttributeString(option);

        await Assert.That(attribute)
            .IsEqualTo("CliOption(\"--arguments\", GroupValues = true)");
    }

    [Test]
    public async Task GenerateCliAttributeString_Includes_Collection_Separator()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--environment",
            PropertyName = "Environment",
            CSharpType = "IReadOnlyList<KeyValue>?",
            CollectionSeparator = ",",
        };

        var attribute = GeneratorUtils.GenerateCliAttributeString(option);

        await Assert.That(attribute)
            .IsEqualTo("CliOption(\"--environment\", CollectionSeparator = \",\")");
    }

    [Test]
    public async Task GenerateCliAttributeString_Rejects_Grouping_With_NonSpace_Separator()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--arguments",
            PropertyName = "Arguments",
            CSharpType = "string[]?",
            GroupValues = true,
            ValueSeparator = "=",
        };

        await Assert.That(() => GeneratorUtils.GenerateCliAttributeString(option))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("must use a space separator");
    }

    [Test]
    public async Task GenerateCliAttributeString_Rejects_Grouping_With_Collection_Separator()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--arguments",
            PropertyName = "Arguments",
            CSharpType = "string[]?",
            GroupValues = true,
            CollectionSeparator = ",",
        };

        await Assert.That(() => GeneratorUtils.GenerateCliAttributeString(option))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("cannot set both GroupValues and CollectionSeparator");
    }

    [Test]
    public async Task GenerateCliAttributeString_Rejects_Unsupported_Separator()
    {
        var option = new CliOptionDefinition
        {
            SwitchName = "--output",
            PropertyName = "Output",
            CSharpType = "string?",
            ValueSeparator = "::",
        };

        await Assert.That(() => GeneratorUtils.GenerateCliAttributeString(option))
            .Throws<InvalidOperationException>()
            .And.HasMessageContaining("Unsupported value separator");
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
        // Empty array returns "Execute" for single-command tools (e.g., ansible)
        var result = GeneratorUtils.GenerateMethodNameFromCommandParts([]);

        await Assert.That(result).IsEqualTo("Execute");
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

        await Assert.That(sb.ToString()).Contains("[RegularExpression(\"^[a-z]+$\")]");
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

    #region Command Signature Tests

    [Test]
    public async Task Aliased_Constructor_Parameter_Types_Preserve_Nullability()
    {
        var enumDefinition = new CliEnumDefinition
        {
            EnumName = "ToolBuildxBakeMode",
            Values = [],
        };
        var enumOption = new CliOptionDefinition
        {
            SwitchName = "--mode",
            PropertyName = "Mode",
            CSharpType = "ToolBuildxBakeMode?",
            EnumDefinition = enumDefinition,
        };
        var command = new CliCommandDefinition
        {
            FullCommand = "tool buildx bake",
            CommandParts = ["buildx", "bake"],
            ClassName = "ToolBuildxBakeOptions",
            ParentClassName = "ToolOptions",
            ToolNamespacePrefix = "Tool",
            Options = [],
            SubDomainGroup = "Buildx",
        };
        var tool = new CliToolDefinition
        {
            ToolName = "tool",
            NamespacePrefix = "Tool",
            TargetNamespace = "ModularPipelines.Tool",
            OutputDirectory = "src/ModularPipelines.Tool",
            Commands = [command],
        };
        var alias = new CliCommandGroupAlias
        {
            Alias = "builder",
            CanonicalCommand = "buildx",
            ObsoleteMessage = "Use Buildx.",
        };
        var nonEnumParameter = new GeneratorUtils.RequiredConstructorParameter(
            "Input",
            "string?",
            IsSecret: false,
            Option: null,
            PositionalArgument: null);
        var enumParameter = new GeneratorUtils.RequiredConstructorParameter(
            "Mode",
            "ToolBuildxBakeMode?",
            IsSecret: false,
            Option: enumOption,
            PositionalArgument: null);

        using (Assert.Multiple())
        {
            await Assert.That(GeneratorUtils.GetAliasedRequiredConstructorParameterType(
                    nonEnumParameter,
                    tool,
                    alias))
                .IsEqualTo("string?");
            await Assert.That(GeneratorUtils.GetAliasedRequiredConstructorParameterType(
                    enumParameter,
                    tool,
                    alias))
                .IsEqualTo("ToolBuilderBakeMode?");
        }
    }

    #endregion

    #region IsSecretOption Tests

    [Test]
    [Arguments("Password")]
    [Arguments("password")]
    [Arguments("PASSWORD")]
    [Arguments("UserPassword")]
    [Arguments("PasswordHash")]
    public async Task IsSecretOption_Returns_True_For_Password_Variants(string propertyName)
    {
        var result = GeneratorUtils.IsSecretOption(propertyName, isFlag: false);

        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("Secret")]
    [Arguments("ClientSecret")]
    [Arguments("SecretKey")]
    [Arguments("MySecretValue")]
    public async Task IsSecretOption_Returns_True_For_Secret_Variants(string propertyName)
    {
        var result = GeneratorUtils.IsSecretOption(propertyName, isFlag: false);

        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("Token")]
    [Arguments("AccessToken")]
    [Arguments("RefreshToken")]
    [Arguments("BearerToken")]
    public async Task IsSecretOption_Returns_True_For_Token_Variants(string propertyName)
    {
        var result = GeneratorUtils.IsSecretOption(propertyName, isFlag: false);

        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("Credential")]
    [Arguments("UserCredential")]
    [Arguments("Creds")]
    [Arguments("RegistryCreds")]
    [Arguments("DestCreds")]
    [Arguments("SrcCreds")]
    public async Task IsSecretOption_Returns_True_For_Credential_Variants(string propertyName)
    {
        var result = GeneratorUtils.IsSecretOption(propertyName, isFlag: false);

        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("Otp")]
    [Arguments("OTP")]
    [Arguments("RegistryOtp")]
    [Arguments("OtpCode")]
    [Arguments("RegistryOtpCode")]
    public async Task IsSecretOption_Returns_True_For_Otp_Variants(string propertyName)
    {
        var result = GeneratorUtils.IsSecretOption(propertyName, isFlag: false);

        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("ApiKey")]
    [Arguments("MyApiKey")]
    [Arguments("ApiKeyValue")]
    public async Task IsSecretOption_Returns_True_For_ApiKey_Variants(string propertyName)
    {
        var result = GeneratorUtils.IsSecretOption(propertyName, isFlag: false);

        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("PrivateKey")]
    [Arguments("SshPrivateKey")]
    public async Task IsSecretOption_Returns_True_For_PrivateKey_Variants(string propertyName)
    {
        var result = GeneratorUtils.IsSecretOption(propertyName, isFlag: false);

        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("AccessKey")]
    [Arguments("AwsAccessKey")]
    public async Task IsSecretOption_Returns_True_For_AccessKey_Variants(string propertyName)
    {
        var result = GeneratorUtils.IsSecretOption(propertyName, isFlag: false);

        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("SecretKey")]
    [Arguments("AwsSecretKey")]
    public async Task IsSecretOption_Returns_True_For_SecretKey_Variants(string propertyName)
    {
        var result = GeneratorUtils.IsSecretOption(propertyName, isFlag: false);

        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("AccountKey")]
    [Arguments("StorageAccountKey")]
    [Arguments("ConnectionString")]
    [Arguments("StorageConnectionString")]
    public async Task IsSecretOption_Returns_True_For_Storage_Credential_Variants(string propertyName)
    {
        var result = GeneratorUtils.IsSecretOption(propertyName, isFlag: false);

        await Assert.That(result).IsTrue();
    }

    [Test]
    [Arguments("Output")]
    [Arguments("Verbose")]
    [Arguments("Format")]
    [Arguments("ConfigFile")]
    [Arguments("PrivateKeyFile")]
    [Arguments("AccessTokenPath")]
    [Arguments("CredentialKeyring")]
    [Arguments("SecretDir")]
    [Arguments("Namespace")]
    [Arguments("Repository")]
    [Arguments("SecretsProvider")]
    [Arguments("NewSecretsProvider")]
    [Arguments("ExecutorRootPath")]
    [Arguments("GrpcWebRootPath")]
    [Arguments("RdbSnapshotPeriod")]
    [Arguments("AutopilotPrivilegedAdmission")]
    [Arguments("CredsHelper")]
    public async Task IsSecretOption_Returns_False_For_Non_Secret_Names(string propertyName)
    {
        var result = GeneratorUtils.IsSecretOption(propertyName, isFlag: false);

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task IsSecretOption_Returns_False_When_Description_Identifies_A_Path()
    {
        var result = GeneratorUtils.IsSecretOption(
            "PrivateKeyLocation",
            isFlag: false,
            "Path to the private key file.");

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task IsSecretOption_Returns_False_For_Flags_Even_With_Secret_Name()
    {
        var result = GeneratorUtils.IsSecretOption("ShowPassword", isFlag: true);

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task IsSecretOption_Returns_False_For_Empty_PropertyName()
    {
        var result = GeneratorUtils.IsSecretOption("", isFlag: false);

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task IsSecretOption_Returns_False_For_Null_PropertyName()
    {
        var result = GeneratorUtils.IsSecretOption(null!, isFlag: false);

        await Assert.That(result).IsFalse();
    }

    [Test]
    public async Task IsSecretOption_Is_Case_Insensitive()
    {
        var lowerResult = GeneratorUtils.IsSecretOption("password", isFlag: false);
        var upperResult = GeneratorUtils.IsSecretOption("PASSWORD", isFlag: false);
        var mixedResult = GeneratorUtils.IsSecretOption("PaSsWoRd", isFlag: false);

        await Assert.That(lowerResult).IsTrue();
        await Assert.That(upperResult).IsTrue();
        await Assert.That(mixedResult).IsTrue();
    }

    #endregion
}
