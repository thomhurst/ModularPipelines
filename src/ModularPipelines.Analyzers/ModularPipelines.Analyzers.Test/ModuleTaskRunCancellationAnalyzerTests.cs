using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleTaskRunCancellationAnalyzerTests
{
    [TestMethod]
    [DataRow("token", true)]
    [DataRow("", false)]
    [DataRow("CancellationToken.None", false)]
    [DataRow("unrelatedToken", false)]
    public async Task Checks_Captured_Helper_Token_In_TaskRun_Lambda(string argument, bool forwardsToken)
    {
        var call = $"CreateTemplateXml({argument})";
        if (!forwardsToken)
        {
            call = $"{{|#0:{call}|}}";
        }

        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                CreateTemplateZip(CancellationToken.None, cancellationToken);

            private static Task<string> CreateTemplateZip(CancellationToken unrelatedToken, CancellationToken token) =>
                Task.Run(async () =>
                {
                    return await {{call}};
                }, token);

            private static Task<string> CreateTemplateXml(CancellationToken token = default) => Task.FromResult("xml");
            """);

        await VerifyCallAsync(source, forwardsToken, "CreateTemplateXml");
    }

    [TestMethod]
    [DataRow("cancellationToken", true)]
    [DataRow("", false)]
    [DataRow("CancellationToken.None", false)]
    [DataRow("new CancellationToken()", false)]
    public async Task Checks_Module_Token_In_TaskRun_Lambda(string argument, bool forwardsToken)
    {
        var call = string.IsNullOrEmpty(argument) ? "Task.Delay(1000)" : $"Task.Delay(1000, {argument})";
        if (!forwardsToken)
        {
            call = $"{{|#0:{call}|}}";
        }

        var source = ModuleSource($$"""
            protected override async Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            {
                await Task.Run(async () =>
                {
                    await {{call}};
                }, cancellationToken);
            }
            """);

        await VerifyCallAsync(source, forwardsToken, "Delay");
    }

    [TestMethod]
    [DataRow("async () => await Task.Delay(1, token)")]
    [DataRow("() => Task.Delay(1, token)")]
    [DataRow("async delegate { await Task.Delay(1, token); }")]
    [DataRow("Work")]
    public async Task Recognizes_Helper_Token_Captured_By_Nested_Callables(string callback)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static async Task HelperAsync(CancellationToken token)
            {
                await Task.Run({{callback}}, token);

                Task Work() => Task.Delay(1, token);
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("execute", false)]
    [DataRow("execute", true)]
    [DataRow("lifecycle", false)]
    [DataRow("lifecycle", true)]
    [DataRow("configuration", false)]
    [DataRow("configuration", true)]
    public async Task Checks_Helper_Token_In_Nested_Lambda_For_Each_Cancellation_Root(string root, bool forwardsToken)
    {
        var entry = root switch
        {
            "execute" => "protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);",
            "lifecycle" => "protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);",
            _ => "protected override void Configure(ModuleConfigurationBuilder module) => module.WithSkipWhen(async (context, cancellationToken) => { await HelperAsync(cancellationToken); return false; });",
        };
        var call = forwardsToken ? "Task.Delay(1, token)" : "{|#0:Task.Delay(1)|}";
        var source = ModuleSource($$"""
            {{(root == "execute" ? "" : "protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.CompletedTask;")}}
            {{entry}}

            private static Task HelperAsync(CancellationToken token)
            {
                return RunAsync();

                Task RunAsync() => Task.Run(async () =>
                {
                    await {{call}};
                }, token);
            }
            """);

        await VerifyCallAsync(source, forwardsToken, "Delay");
    }

    [TestMethod]
    public async Task Reports_Helper_Token_Not_Flowed_On_Every_Call_Path()
    {
        var source = ModuleSource("""
            protected override async Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            {
                await HelperAsync(cancellationToken, CancellationToken.None);
                await HelperAsync(CancellationToken.None, cancellationToken);
            }

            private static Task HelperAsync(CancellationToken firstToken, CancellationToken secondToken) =>
                {|#1:Task.Run(async () =>
                {
                    await {|#0:Task.Delay(1, firstToken)|};
                }, firstToken)|};
            """);

        // Neither helper parameter is guaranteed to carry the module token.
        // Task.Run and its awaited callback must both be diagnosed.
        await VerifyCS.VerifyAnalyzerAsync(
            source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(1).WithArguments("Run"));
    }

    [TestMethod]
    [DataRow("token = CancellationToken.None;", false)]
    [DataRow("token = default;", false)]
    [DataRow("Reset(ref token);", false)]
    [DataRow("ResetOut(out token);", false)]
    [DataRow("(token, _) = (CancellationToken.None, 0);", false)]
    [DataRow("token = CancellationToken.None;", true)]
    [DataRow("void ResetLocal() { token = default; } ResetLocal();", false)]
    [DataRow("void ResetLocal() { token = default; } Action reset = ResetLocal; reset();", false)]
    [DataRow("Action reset = () => token = default; Store(reset);", false)]
    [DataRow("void ResetLocal<T>() { token = default; } Store(ResetLocal<int>);", false)]
    [DataRow("var other = default(CancellationToken); (Environment.TickCount > 0 ? ref token : ref other) = default;", false)]
    [DataRow("ref var alias = ref token; alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; alias = (alias = ref other);", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; (alias, _) = ((alias = ref other), 0);", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; alias = ref (alias = ref token); alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; alias = ref token; alias = default;", false)]
    [DataRow("ref var first = ref token; ref var second = ref first; second = default;", false)]
    [DataRow("ref var alias = ref token; Reset(ref alias);", false)]
    [DataRow("ref var alias = ref token; ResetOut(out alias);", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; alias = ref (Environment.TickCount > 0 ? ref token : ref other); alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { alias = default; alias = ref token; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { alias = ref token; if (Environment.TickCount > 0) break; alias = ref other; } alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; for (; Environment.TickCount > 0; alias = ref other) { alias = ref token; if (Environment.TickCount > 0) break; } alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; do { alias = ref token; if (Environment.TickCount > 0) break; alias = ref other; } while (Environment.TickCount > 0); alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { alias = default; alias = ref token; if (Environment.TickCount > 0) continue; alias = ref other; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { try { alias = ref other; break; } finally { alias = ref token; } } alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { try { alias = ref token; break; } finally { alias = default; } }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { while (Environment.TickCount > 0) { alias = ref token; break; } if (Environment.TickCount > 0) break; alias = ref other; } alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; for (alias = ref other; Environment.TickCount > 0;) { alias = default; alias = ref token; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; if (Environment.TickCount > 0) { alias = ref other; } alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; if (Environment.TickCount > 0) { alias = ref other; } else { alias = default; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; try { alias = ref other; } finally { token = default; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; switch (Environment.TickCount) { case 0: alias = ref other; break; } alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; _ = Environment.TickCount > 0 && (alias = ref other).CanBeCanceled; alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; try { Read(token); alias = ref other; } finally { alias = default; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; again: alias = default; alias = ref token; if (Environment.TickCount > 0) goto again;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; _ = Environment.GetEnvironmentVariable(\"X\") ?? (alias = ref other).ToString(); alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; _ = Environment.GetEnvironmentVariable(\"X\")?.Contains((alias = ref other).ToString()!); alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; var value = Environment.GetEnvironmentVariable(\"X\"); value ??= (alias = ref other).ToString(); alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; switch (Environment.TickCount) { case int n when (alias = ref token).CanBeCanceled && n > 0: break; default: alias = default; break; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; switch (Environment.TickCount) { default: alias = default; break; case int n when (alias = ref token).CanBeCanceled && n > 0: break; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; _ = Environment.TickCount switch { int n when (alias = ref token).CanBeCanceled && n > 0 => 0, _ => (alias = default).GetHashCode() };", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; try { throw new Exception(); } catch (Exception) when ((alias = ref token).CanBeCanceled && Environment.TickCount > 0) { } catch (Exception) { alias = default; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; try { Read(token); } catch (InvalidOperationException) when ((alias = ref token).CanBeCanceled && Environment.TickCount > 0) { } catch (Exception) { alias = default; }", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; try { throw new Exception(); } catch (Exception) when ((alias = ref token).CanBeCanceled && Environment.TickCount > 0) { } catch (Exception) { } alias = default;", false)]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; try { Read(token); } catch (InvalidOperationException) when ((alias = ref other).CanBeCanceled) { } catch (Exception) { alias = default; }", false)]
    public async Task Reports_Overwritten_Helper_Token_Captured_By_Callback(string write, bool insideCallback)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                {{(insideCallback ? "" : write)}}
                return Task.Run(async () =>
                {
                    {{(insideCallback ? write : "")}}
                    await {|#0:Task.Delay(1, token)|};
                }, token);
            }

            private static void Read(CancellationToken token) { }
            private static void Reset(ref CancellationToken token) => token = default;
            private static void ResetOut(out CancellationToken token) => token = default;
            private static void Store(Action callback) { }
            """);

        await VerifyCallAsync(source, false, "Delay");
    }

    [TestMethod]
    [DataRow("Read(in token);")]
    [DataRow("if (false) { token = default; }")]
    [DataRow("void ResetLocal() { token = default; }")]
    [DataRow("var other = default(CancellationToken); (false ? ref token : ref other) = default;")]
    [DataRow("ref readonly var alias = ref token; _ = alias.CanBeCanceled;")]
    [DataRow("ref var alias = ref token; _ = alias.CanBeCanceled;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; alias = (alias = ref token);")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; (alias, _) = ((alias = ref token), 0);")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; alias = ref (alias = ref other); alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; switch (Environment.TickCount) { case int n when (alias = ref token).CanBeCanceled && n > 0: break; case var _ when (alias = ref other).CanBeCanceled: break; default: alias = default; break; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; switch (Environment.TickCount) { case int n when (alias = ref other).CanBeCanceled && n > 0: break; default: alias = default; break; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; _ = Environment.TickCount switch { int n when (alias = ref token).CanBeCanceled && n > 0 => 0, var _ when (alias = ref other).CanBeCanceled => 1, _ => (alias = default).GetHashCode() };")]
    [DataRow("ref var alias = ref token; Read(in alias);")]
    [DataRow("ref var alias = ref token; if (false) { alias = default; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; alias = ref token; _ = alias.CanBeCanceled;")]
    [DataRow("ref var first = ref token; ref var second = ref first; _ = second.CanBeCanceled;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref (false ? ref token : ref other); alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; alias = default; alias = ref token;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; ReadWrite(ref alias); alias = ref token;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; ref var second = ref alias; second = default; alias = ref token;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; alias = ref other; alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { alias = ref token; alias = ref other; if (Environment.TickCount > 0) break; } alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { alias = ref token; alias = ref other; if (Environment.TickCount > 0) continue; alias = default; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { switch (Environment.TickCount) { case 0: alias = ref token; break; } alias = ref other; } alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; for (; Environment.TickCount > 0; alias = ref other) { alias = default; alias = ref token; continue; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { try { alias = ref token; break; } finally { alias = ref other; } } alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; alias = ref other; ref var second = ref alias; second = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; if (Environment.TickCount > 0) { alias = ref other; } else { alias = ref other; } alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; if (true) { alias = ref other; } alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; while (Environment.TickCount > 0) { alias = ref other; alias = default; alias = ref token; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; switch (Environment.TickCount) { case 0: alias = ref other; break; default: alias = ref other; break; } alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; try { alias = default; alias = ref token; } finally { Read(in token); }")]
    [DataRow("var dictionary = new System.Collections.Generic.Dictionary<CancellationToken, int>(); dictionary[token] = 1;")]
    [DataRow("var dictionary = new System.Collections.Generic.Dictionary<CancellationToken, int>(); (dictionary[token], _) = (1, 2);")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; _ = Environment.GetEnvironmentVariable(\"X\") ?? (alias = ref other).ToString(); alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; _ = Environment.GetEnvironmentVariable(\"X\")?.Contains((alias = ref other).ToString()!); alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; switch (Environment.TickCount) { case int n when (alias = ref other).CanBeCanceled && n > 0: alias = default; break; default: alias = ref other; alias = default; break; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; _ = Environment.TickCount switch { int n when (alias = ref other).CanBeCanceled && n > 0 => (alias = default).GetHashCode(), _ => (alias = ref other).GetHashCode() }; alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; _ = ((string?)null) ?? (alias = ref other).ToString(); alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; _ = \"present\" ?? (alias = ref token).ToString(); alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; _ = \"present\"?.Contains((alias = ref other).ToString()!); alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; _ = ((string?)null)?.Contains((alias = ref token).ToString()!); alias = default;")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref token; try { throw new Exception(); } catch (Exception) when ((alias = ref other).CanBeCanceled && Environment.TickCount > 0) { } catch (Exception) { alias = default; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; try { throw new Exception(); } catch (Exception) when ((alias = ref token).CanBeCanceled && Environment.TickCount > 0) { } catch (Exception) when ((alias = ref other).CanBeCanceled) { } catch (Exception) { alias = default; }")]
    [DataRow("var other = default(CancellationToken); ref var alias = ref other; try { throw new Exception(); } catch (Exception) when ((alias = ref token).CanBeCanceled && Environment.TickCount > 0) { } catch (Exception) { alias = ref other; alias = default; }")]
    public async Task Recognizes_Unmodified_Helper_Token_Captured_By_Callback(string statement)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                {{statement}}
                return Task.Run(async () => await Task.Delay(1, token), token);
            }

            private static void Read(in CancellationToken token) { }
            private static void ReadWrite(ref CancellationToken token) { }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("tokens[0] == (alias = ref other)", false, true)]
    [DataRow("MayThrow() | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("(alias = ref token).CanBeCanceled | tokens[0].CanBeCanceled | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("(1 / tokens.Length > 0) | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("tokens[0] == (alias = ref other)", true, false)]
    [DataRow("(alias = ref other).CanBeCanceled | MayThrow()", false, false)]
    [DataRow("(alias = ref other).CanBeCanceled | tokens[0].CanBeCanceled", false, false)]
    [DataRow("tokens[(alias = ref other).GetHashCode()] == other", false, false)]
    [DataRow("choice switch { 1 => (alias = ref other).CanBeCanceled }", false, true)]
    [DataRow("choice switch { _ => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("checked(++number) > 0 | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("(alias = ref other).CanBeCanceled | checked(++number) > 0", false, false)]
    [DataRow("(long)number > 0 | (alias = ref other).CanBeCanceled", false, false)]
    [DataRow("unchecked(++number) > 0 | (alias = ref other).CanBeCanceled", false, false)]
    [DataRow("checked((long)number) > 0 | (alias = ref other).CanBeCanceled", false, false)]
    [DataRow("checked((byte)number) > 0 | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("((object?)tokens is null) | (alias = ref other).CanBeCanceled", false, false)]
    [DataRow("((CancellationToken)(object)tokens).CanBeCanceled | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("(choice > 0) switch { true => (alias = ref other).CanBeCanceled, false => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("(choice > 0) switch { true when choice > 1 => (alias = ref other).CanBeCanceled, false => (alias = ref other).CanBeCanceled }", false, true)]
    public async Task Tracks_Throwing_Exception_Filter_Bindings(string filter, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                var tokens = Array.Empty<CancellationToken>();
                var choice = Environment.TickCount;
                var number = int.MaxValue;
                try { throw new Exception(); }
                catch (Exception) when ({{filter}}) { }
                catch (Exception)
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private static bool MayThrow() => throw new Exception();
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    [DataRow("state switch { { Value: true } => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, true)]
    [DataRow("state switch { (true, _) => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, true)]
    [DataRow("state switch { [true] => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, true)]
    [DataRow("state switch { [.. [true]] => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, true)]
    [DataRow("state is (true, _) | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("state is [true] | (alias = ref other).CanBeCanceled", false, true)]
    [DataRow("state switch { { Value: true } => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", true, false)]
    [DataRow("(alias = ref other).CanBeCanceled | (state switch { { Value: true } => false, _ => false })", false, false)]
    [DataRow("state switch { not null => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("state switch { PatternState _ => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("state switch { { Field: true } => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("tokens switch { [true] => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("tokens switch { [.. [true]] => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("text switch { ['a'] => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("text switch { [.. ['a']] => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, false)]
    [DataRow("(true, false) switch { (true, _) => (alias = ref other).CanBeCanceled, _ => (alias = ref other).CanBeCanceled }", false, false)]
    public async Task Tracks_Exception_Filter_Pattern_Bindings(string filter, bool rebindInHandler, bool reports)
    {
        var source = ModuleSource($$"""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var other = default(CancellationToken);
                ref var alias = ref token;
                var state = new PatternState();
                var tokens = Array.Empty<bool>();
                var text = "text";
                try { throw new Exception(); }
                catch (Exception) when ({{filter}}) { }
                catch (Exception)
                {
                    {{(rebindInHandler ? "alias = ref other;" : "")}}
                    alias = default;
                }

                return Task.Run(async () => await {{(reports ? "{|#0:Task.Delay(1, token)|}" : "Task.Delay(1, token)")}}, token);
            }

            private sealed class PatternState
            {
                public bool Value => throw new Exception();
                public bool Field;
                public int Length => throw new Exception();
                public bool this[int index] => throw new Exception();
                public PatternState Slice(int start, int length) => throw new Exception();
                public void Deconstruct(out bool first, out bool second) => throw new Exception();
            }
            """);

        await VerifyCallAsync(source, !reports, "Delay");
    }

    [TestMethod]
    public async Task Reports_Helper_Token_Changed_After_Callback_Is_Scheduled()
    {
        var source = ModuleSource("""
            protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                HelperAsync(cancellationToken);

            private static Task HelperAsync(CancellationToken token)
            {
                var work = Task.Run(async () => await {|#0:Task.Delay(1, token)|}, token);
                token = default;
                return work;
            }
            """);

        await VerifyCallAsync(source, false, "Delay");
    }

    private static string ModuleSource(string members) => $$"""
        {{TestSourceConstants.StandardModuleHeader}}

        public sealed class DummyModule : Module
        {
            {{members}}
        }
        """;

    private static Task VerifyCallAsync(string source, bool forwardsToken, string methodName) =>
        forwardsToken
            ? VerifyCS.VerifyAnalyzerAsync(source)
            : VerifyCS.VerifyAnalyzerAsync(
                source,
                VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                    .WithLocation(0)
                    .WithArguments(methodName));
}
