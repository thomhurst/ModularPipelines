# yq CLI reference

`ModularPipelines.Yq` provides strongly typed access to the `yq` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `yq` executable. Install it separately and ensure `yq` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Yq
```

Resolve the service with `context.Tools.Yq`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.Yq.Services.IYq>()` instead.

## Module example[​](#module-example "Direct link to Module example")

Resolve the service in a module, then select a command from the table below. A runnable example is omitted when no command has complete safety metadata:

```
var yq = context.Tools.Yq;
```

## Global options[​](#global-options "Direct link to Global options")

Global options are rendered before the selected subcommand.

| CLI option                          | Property                       | Availability | Description                                                                                                                                                                                      |
| ----------------------------------- | ------------------------------ | ------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| `--colors`                          | `Colors`                       | All editions | force print with colors                                                                                                                                                                          |
| `--csv-auto-parse`                  | `CsvAutoParse`                 | All editions | parse CSV YAML/JSON values (default true)                                                                                                                                                        |
| `--csv-separator`                   | `CsvSeparator`                 | All editions | CSV Separator character (default ,)                                                                                                                                                              |
| `--debug-node-info`                 | `DebugNodeInfo`                | All editions | debug node info                                                                                                                                                                                  |
| `--exit-status`                     | `ExitStatus`                   | All editions | set exit status if there are no matches or null or false is returned                                                                                                                             |
| `--expression`                      | `Expression`                   | All editions | forcibly set the expression argument. Useful when yq argument detection thinks your expression is a file.                                                                                        |
| `--from-file`                       | `FromFile`                     | All editions | Load expression from specified file.                                                                                                                                                             |
| `--front-matter`                    | `FrontMatter`                  | All editions | (extract\|process) first input as yaml front-matter. Extract will pull out the yaml content, process will run the expression against the yaml content, leaving the remaining data intact         |
| `--header-preprocess`               | `HeaderPreprocess`             | All editions | Slurp any header comments and separators before processing expression. (default true)                                                                                                            |
| `--indent`                          | `Indent`                       | All editions | sets indent level for output (default 2)                                                                                                                                                         |
| `--ini-preserve-quotes`             | `IniPreserveQuotes`            | All editions | preserve surrounding quotes on INI values during round-trip                                                                                                                                      |
| `--inplace`                         | `Inplace`                      | All editions | update the file in place of first file given.                                                                                                                                                    |
| `--input-format`                    | `InputFormat`                  | All editions | \[auto\|a\|yaml\|y\|kyaml\|ky\|json\|j\|props\|p\|csv\|c\|tsv\|t\|xml\|x\|base64\|base64url\|uri\|toml\|hcl\|h\|lua\|l\|ini\|i] parse format for input. (default "auto")                         |
| `--lua-globals`                     | `LuaGlobals`                   | All editions | output keys as top-level global variables                                                                                                                                                        |
| `--lua-prefix`                      | `LuaPrefix`                    | All editions | prefix (default "return ")                                                                                                                                                                       |
| `--lua-suffix`                      | `LuaSuffix`                    | All editions | suffix (default ";\n")                                                                                                                                                                           |
| `--lua-unquoted`                    | `LuaUnquoted`                  | All editions | output unquoted string keys (e.g. {foo="bar"})                                                                                                                                                   |
| `--no-colors`                       | `NoColors`                     | All editions | force print with no colors                                                                                                                                                                       |
| `--no-doc`                          | `NoDoc`                        | All editions | Don't print document separators (---)                                                                                                                                                            |
| `--nul-output`                      | `NulOutput`                    | All editions | Use NUL char to separate values. If unwrap scalar is also set, fail if unwrapped scalar contains NUL char.                                                                                       |
| `--null-input`                      | `NullInput`                    | All editions | Don't read input, simply evaluate the expression given. Useful for creating docs from scratch.                                                                                                   |
| `--output-format`                   | `OutputFormat`                 | All editions | \[auto\|a\|yaml\|y\|kyaml\|ky\|json\|j\|props\|p\|csv\|c\|tsv\|t\|xml\|x\|base64\|base64url\|uri\|toml\|hcl\|h\|shell\|s\|lua\|l\|ini\|i] output format type. (default "auto")                   |
| `--prettyPrint`                     | `PrettyPrint`                  | All editions | pretty print, shorthand for '... style = ""'                                                                                                                                                     |
| `--properties-array-brackets`       | `PropertiesArrayBrackets`      | All editions | use \[x] in array paths (e.g. for SpringBoot)                                                                                                                                                    |
| `--properties-separator`            | `PropertiesSeparator`          | All editions | separator to use between keys and values (default " = ")                                                                                                                                         |
| `--security-disable-env-ops`        | `SecurityDisableEnvOps`        | All editions | Disable env related operations.                                                                                                                                                                  |
| `--security-disable-file-ops`       | `SecurityDisableFileOps`       | All editions | Disable file related operations (e.g. load)                                                                                                                                                      |
| `--security-enable-system-operator` | `SecurityEnableSystemOperator` | All editions | Enable system operator to allow execution of external commands.                                                                                                                                  |
| `--shell-key-separator`             | `ShellKeySeparator`            | All editions | separator for shell variable key paths (default "\_")                                                                                                                                            |
| `--split-exp`                       | `SplitExp`                     | All editions | print each result (or doc) into a file named (exp). \[exp] argument must return a string. You can use $index in the expression as the result counter. The necessary directories will be created. |
| `--split-exp-file`                  | `SplitExpFile`                 | All editions | Use a file to specify the split-exp expression.                                                                                                                                                  |
| `--string-interpolation`            | `StringInterpolation`          | All editions | Toggles strings interpolation of (exp) (default true)                                                                                                                                            |
| `--tsv-auto-parse`                  | `TsvAutoParse`                 | All editions | parse TSV YAML/JSON values (default true)                                                                                                                                                        |
| `--unwrapScalar`                    | `UnwrapScalar`                 | All editions | unwrap scalar, print the value with no quotes, colours or comments. Defaults to true for yaml (default true)                                                                                     |
| `--verbose`                         | `Verbose`                      | All editions | verbose mode                                                                                                                                                                                     |
| `--xml-attribute-prefix`            | `XmlAttributePrefix`           | All editions | prefix for xml attributes (default "+@")                                                                                                                                                         |
| `--xml-content-name`                | `XmlContentName`               | All editions | name for xml content (if no attribute name is present). (default "+content")                                                                                                                     |
| `--xml-directive-name`              | `XmlDirectiveName`             | All editions | name for xml directives (e.g. \<!DOCTYPE thing cat>) (default "+directive")                                                                                                                      |
| `--xml-keep-namespace`              | `XmlKeepNamespace`             | All editions | enables keeping namespace after parsing attributes (default true)                                                                                                                                |
| `--xml-proc-inst-prefix`            | `XmlProcInstPrefix`            | All editions | prefix for xml processing instructions (e.g. \<?xml version="1"?>) (default "+p\_")                                                                                                              |
| `--xml-raw-token`                   | `XmlRawToken`                  | All editions | enables using RawToken method instead Token. Commonly disables namespace translations. See <https://pkg.go.dev/encoding/xml#Decoder.RawToken> for details. (default true)                        |
| `--xml-skip-directives`             | `XmlSkipDirectives`            | All editions | skip over directives (e.g. \<!DOCTYPE thing cat>)                                                                                                                                                |
| `--xml-skip-proc-inst`              | `XmlSkipProcInst`              | All editions | skip over process instructions (e.g. \<?xml version="1"?>)                                                                                                                                       |
| `--xml-strict-mode`                 | `XmlStrictMode`                | All editions | enables strict parsing of XML. See <https://pkg.go.dev/encoding/xml> for more details.                                                                                                           |
| `--yaml-compact-seq-indent`         | `YamlCompactSeqIndent`         | All editions | Use compact sequence indentation where '- ' is considered part of the indentation.                                                                                                               |
| `--yaml-fix-merge-anchor-to-spec`   | `YamlFixMergeAnchorToSpec`     | All editions | Fix merge anchor to match YAML spec. Will default to true in late 2025                                                                                                                           |

## Commands[​](#commands "Direct link to Commands")

| CLI command   | Options record     |
| ------------- | ------------------ |
| `yq eval`     | `YqEvalOptions`    |
| `yq eval-all` | `YqEvalAllOptions` |
