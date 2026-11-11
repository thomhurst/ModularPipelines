using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleCancellationTokenParameterAnalyzerTests
{
    [TestMethod]
    [DataRow("cancellationToken = CancellationToken.None;")]
    [DataRow("cancellationToken = new CancellationToken();")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; if (context != null) { cancellationToken = saved; }")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; if (context != null) { cancellationToken = saved; } else { cancellationToken = CancellationToken.None; }")]
    [DataRow("if (DateTime.Now.Ticks > 0) cancellationToken = CancellationToken.None;")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; saved = cancellationToken; cancellationToken = saved;")]
    public async Task Reports_Replaced_Parameter(string statements)
    {
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "{|#0:Task.Delay(1, cancellationToken)|}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow("")]
    [DataRow("cancellationToken = cancellationToken;")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; if (context != null) { cancellationToken = saved; } else { cancellationToken = saved; }")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; if (context != null) { if (DateTime.Now.Ticks > 0) { cancellationToken = saved; } else { cancellationToken = saved; } } else { cancellationToken = saved; }")]
    [DataRow("var saved = cancellationToken; cancellationToken = saved;")]
    [DataRow("var saved = cancellationToken; cancellationToken = CancellationToken.None; cancellationToken = saved;")]
    [DataRow("if (false) cancellationToken = CancellationToken.None;")]
    [DataRow("var saved = cancellationToken; if (context != null) { cancellationToken = CancellationToken.None; cancellationToken = saved; }")]
    [DataRow("var saved = cancellationToken; cancellationToken = saved; saved = cancellationToken; cancellationToken = saved;")]
    [DataRow("var saved = cancellationToken; if (context != null) { saved = CancellationToken.None; saved = cancellationToken; } cancellationToken = saved;")]
    [DataRow("var saved = CancellationToken.None; if (context != null) { saved = cancellationToken; } else { saved = cancellationToken; } cancellationToken = saved;")]
    public async Task Recognizes_Original_Parameter_Value(string statements)
    {
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, "Task.Delay(1, cancellationToken)"));
    }

    [TestMethod]
    public async Task Reports_Replaced_Parameter_Before_Branch_Restoration()
    {
        var source = Source(
                "var saved = cancellationToken; cancellationToken = CancellationToken.None; if (context != null) { await {|#0:Task.Delay(1, cancellationToken)|}; cancellationToken = saved; } else { cancellationToken = saved; }",
                "Task.Delay(1, cancellationToken)")
            .Replace("override Task OnBeforeExecuteAsync", "override async Task OnBeforeExecuteAsync")
            .Replace("return Task.Delay(1, cancellationToken);", "await Task.Delay(1, cancellationToken);");
        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    public async Task Recognizes_Original_Parameter_In_Exclusive_Branch()
    {
        await VerifyCS.VerifyAnalyzerAsync(Source(
            "if (context != null) { cancellationToken = CancellationToken.None; } else { return Task.Delay(1, cancellationToken); }",
            "Task.CompletedTask"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Recognizes_Original_Token_In_Exclusive_Switch_Case(bool useLocal)
    {
        var target = useLocal ? "saved" : "cancellationToken";
        await VerifyCS.VerifyAnalyzerAsync(Source(
            (useLocal ? "var saved = cancellationToken; " : "")
            + $"switch (context) {{ case null: {target} = CancellationToken.None; return Task.CompletedTask; default: return Task.Delay(1, {target}); }}",
            "Task.CompletedTask"));
    }

    [TestMethod]
    [DataRow(false, "goto default;")]
    [DataRow(true, "goto default;")]
    [DataRow(false, "goto case 1;")]
    [DataRow(true, "goto case 1;")]
    [DataRow(false, "goto transfer;")]
    [DataRow(true, "goto transfer;")]
    public async Task Reports_Replaced_Token_After_Switch_Transfer(bool useLocal, string transfer)
    {
        var target = useLocal ? "saved" : "cancellationToken";
        var destination = transfer == "goto default;" ? "default:" : "case 1:";
        var label = transfer == "goto transfer;" ? "transfer:" : "";
        await VerifyCS.VerifyAnalyzerAsync(Source(
            (useLocal ? "var saved = cancellationToken; " : "")
            + $"switch (DateTime.Now.Second) {{ case 0: {target} = CancellationToken.None; {transfer} {destination} {label} return {{|#0:Task.Delay(1, {target})|}}; }}",
            "Task.CompletedTask"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow(false, "if (DateTime.Now.Ticks > 0) break;")]
    [DataRow(true, "if (DateTime.Now.Ticks > 0) break;")]
    [DataRow(false, "if (DateTime.Now.Ticks > 0) goto end;")]
    [DataRow(true, "if (DateTime.Now.Ticks > 0) goto end;")]
    [DataRow(false, "if (DateTime.Now.Ticks > 0) { break; } else { {target} = saved; }")]
    [DataRow(true, "if (DateTime.Now.Ticks > 0) { break; } else { {target} = saved; }")]
    [DataRow(false, "if (DateTime.Now.Ticks > 0) break; return Task.CompletedTask;")]
    [DataRow(true, "if (DateTime.Now.Ticks > 0) break; return Task.CompletedTask;")]
    [DataRow(false, "try { if (DateTime.Now.Ticks > 0) break; {target} = saved; } finally { }")]
    [DataRow(true, "try { if (DateTime.Now.Ticks > 0) break; {target} = saved; } finally { }")]
    public async Task Reports_Replaced_Token_After_Early_Switch_Exit(bool useLocal, string earlyExit)
    {
        var target = useLocal ? "token" : "cancellationToken";
        await VerifyCS.VerifyAnalyzerAsync(Source(
            $"var saved = cancellationToken; {(useLocal ? "var token = cancellationToken; " : "")}{target} = CancellationToken.None; "
            + $"switch (context != null) {{ default: {{ {earlyExit.Replace("{target}", target)} {target} = saved; break; }} }} "
            + (earlyExit.Contains("goto") ? "end:;" : ""),
            $"{{|#0:Task.Delay(1, {target})|}}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow(false, false)]
    [DataRow(true, false)]
    [DataRow(false, true)]
    [DataRow(true, true)]
    public async Task Recognizes_Original_Token_After_Switch_Restoration(bool useLocal, bool returningCase)
    {
        var target = useLocal ? "token" : "cancellationToken";
        await VerifyCS.VerifyAnalyzerAsync(Source(
            $"var saved = cancellationToken; {(useLocal ? "var token = cancellationToken; " : "")}{target} = CancellationToken.None; "
            + $"switch (context) {{ case null: {(returningCase ? "return Task.CompletedTask;" : $"{target} = saved; break;")} default: {{ switch (DateTime.Now.Second) {{ default: break; }} {target} = saved; break; }} }}",
            $"Task.Delay(1, {target})"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Recognizes_Original_Token_After_Nested_Switch_Transfer(bool useLocal)
    {
        var target = useLocal ? "token" : "cancellationToken";
        await VerifyCS.VerifyAnalyzerAsync(Source(
            $"var saved = cancellationToken; {(useLocal ? "var token = cancellationToken; " : "")}{target} = CancellationToken.None; "
            + $"switch (context) {{ case null: {target} = saved; break; default: switch (DateTime.Now.Second) {{ case 0: goto default; default: break; }} {target} = saved; break; }}",
            $"Task.Delay(1, {target})"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Recognizes_Original_Token_After_Returning_Branch(bool useLocal)
    {
        var target = useLocal ? "token" : "cancellationToken";
        await VerifyCS.VerifyAnalyzerAsync(Source(
            (useLocal ? "var token = cancellationToken; " : "")
            + $"if (context != null) {{ {target} = CancellationToken.None; return Task.CompletedTask; }}",
            $"Task.Delay(1, {target})"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Recognizes_Repeated_Conditional_Identity_Assignments(bool useLocal)
    {
        var target = useLocal ? "saved" : "cancellationToken";
        var statements = (useLocal ? "var saved = cancellationToken; " : "")
            + string.Join(" ", Enumerable.Repeat($"if (context != null) {target} = {target};", 32));
        await VerifyCS.VerifyAnalyzerAsync(Source(statements, $"Task.Delay(1, {target})"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Reports_Replaced_Execute_Parameter(bool returnedDirectly)
    {
        var body = returnedDirectly
            ? "return {|#0:Task.Run(() => new List<string>(), cancellationToken)|};"
            : "await {|#0:Task.Delay(1, cancellationToken)|}; return new List<string>();";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}
            public class BuildModule : Module<List<string>>
            {
                protected override {{(returnedDirectly ? "" : "async ")}}Task<List<string>> ExecuteAsync(
                    IModuleContext context, CancellationToken cancellationToken)
                {
                    cancellationToken = CancellationToken.None;
                    {{body}}
                }
            }
            """;
        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments(returnedDirectly ? "Run" : "Delay"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Reports_Replaced_Token_In_Finally_After_Return(bool useLocal)
    {
        var target = useLocal ? "token" : "cancellationToken";
        var source = Source(
                (useLocal ? "var token = cancellationToken; " : "")
                + $"try {{ {target} = CancellationToken.None; return; }} finally {{ await {{|#0:Task.Delay(1, {target})|}}; }}",
                "Task.CompletedTask")
            .Replace("override Task OnBeforeExecuteAsync", "override async Task OnBeforeExecuteAsync")
            .Replace("return Task.CompletedTask;", "return;");
        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Reports_Replaced_Token_When_Internal_Jump_Skips_Restoration(bool useLocal)
    {
        var target = useLocal ? "token" : "cancellationToken";
        await VerifyCS.VerifyAnalyzerAsync(Source(
            $"var saved = cancellationToken; {(useLocal ? "var token = cancellationToken; " : "")}{target} = CancellationToken.None; "
            + $"switch (context) {{ default: try {{ if (context != null) goto skip; {target} = saved; skip: ; }} finally {{ }} break; }}",
            $"{{|#0:Task.Delay(1, {target})|}}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow(false, "case 0: goto default; default: break;")]
    [DataRow(true, "case 0: goto default; default: break;")]
    [DataRow(false, "default: if (context != null) goto skip; skip: break;")]
    [DataRow(true, "default: if (context != null) goto skip; skip: break;")]
    public async Task Recognizes_Original_Token_After_Nested_Switch_In_Try(bool useLocal, string cases)
    {
        var target = useLocal ? "token" : "cancellationToken";
        await VerifyCS.VerifyAnalyzerAsync(Source(
            $"var saved = cancellationToken; {(useLocal ? "var token = cancellationToken; " : "")}{target} = CancellationToken.None; "
            + $"switch (context) {{ default: try {{ switch (DateTime.Now.Second) {{ {cases} }} {target} = saved; }} finally {{ }} break; }}",
            $"Task.Delay(1, {target})"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Recognizes_Original_Token_After_Finally_Restoration(bool useLocal)
    {
        var target = useLocal ? "token" : "cancellationToken";
        await VerifyCS.VerifyAnalyzerAsync(Source(
            $"var saved = cancellationToken; {(useLocal ? "var token = cancellationToken; " : "")}"
            + $"try {{ {target} = CancellationToken.None; }} finally {{ {target} = saved; }}",
            $"Task.Delay(1, {target})"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Reports_Replaced_Token_After_Partial_Finally_Restoration(bool useLocal)
    {
        var target = useLocal ? "token" : "cancellationToken";
        await VerifyCS.VerifyAnalyzerAsync(Source(
            $"var saved = cancellationToken; {(useLocal ? "var token = cancellationToken; " : "")}"
            + $"try {{ {target} = CancellationToken.None; }} finally {{ if (context != null) {target} = saved; }}",
            $"{{|#0:Task.Delay(1, {target})|}}"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Reports_Replaced_Token_In_Catch_After_Implicit_Exception(bool useLocal)
    {
        var target = useLocal ? "token" : "cancellationToken";
        var source = Source(
                (useLocal ? "var token = cancellationToken; " : "")
                + $"try {{ {target} = CancellationToken.None; System.Console.WriteLine(context); return; }} catch (Exception) {{ await {{|#0:Task.Delay(1, {target})|}}; }}",
                "Task.CompletedTask")
            .Replace("override Task OnBeforeExecuteAsync", "override async Task OnBeforeExecuteAsync")
            .Replace("return Task.CompletedTask;", "return;");
        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
    }

    private static string Source(string statements, string call) => $$"""
        {{TestSourceConstants.StandardModuleHeader}}
        public class BuildModule : Module<List<string>>
        {
            {{TestSourceConstants.SimpleAsyncExecuteBody}}
            protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            {
                {{statements}}
                return {{call}};
            }
        }
        """;
}
