import importlib.util
import os
from pathlib import Path
import platform
import subprocess
import sys
import tempfile
import unittest


spec = importlib.util.spec_from_file_location(
    'compiler_trace', Path(__file__).with_name('trace-public-api-compiler.py'))
compiler_trace = importlib.util.module_from_spec(spec)
spec.loader.exec_module(compiler_trace)


class CompilerSelectionTests(unittest.TestCase):
    def test_only_target_project_compilers_owned_by_sync_are_selected(self):
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)

            def process(pid, parent, arguments):
                path = root / str(pid)
                path.mkdir()
                (path / 'status').write_text(f'PPid:\t{parent}\n')
                (path / 'cmdline').write_bytes('\0'.join(arguments).encode())

            command = ['/sdk/Roslyn/bincore/csc', '/out:obj/Release/net10.0/Azure.dll']
            process(100, 1, ['pwsh'])
            process(101, 100, ['dotnet', 'build'])
            process(102, 101, command)
            process(103, 1, command)  # Same project in an unrelated build.
            process(104, 101, [command[0], '/out:obj/Release/net10.0/Core.dll'])
            process(105, 101, ['echo', *command])  # Merely mentions the compiler.
            process(106, 101, ['dotnet', '/sdk/Roslyn/bincore/csc.dll', command[1]])
            response_file = root / 'compiler options.rsp'
            response_file.write_text('/nologo\n/out:"obj/Release/net10.0/Azure.dll"\n', encoding='utf-8-sig')
            process(107, 101, [command[0], '@' + str(response_file)])
            process(108, 1, [command[0], '@' + str(response_file)])
            process(109, 101, [command[0], '@missing.rsp'])
            self.assertEqual([102, 106, 107], sorted(compiler_trace.find_compilers(100, 'Azure', root)))

    def test_disappearing_process_or_parent_cycle_is_not_owned(self):
        with tempfile.TemporaryDirectory() as directory:
            root = Path(directory)
            (root / '101').mkdir()
            (root / '101' / 'status').write_text('PPid:\t101\n')
            self.assertFalse(compiler_trace.is_descendant(101, 100, root))
            self.assertFalse(compiler_trace.is_descendant(102, 100, root))

    @unittest.skipUnless(platform.system() == 'Linux', 'The collector uses Linux /proc.')
    def test_build_failure_exit_code_is_preserved_without_a_compiler(self):
        with tempfile.TemporaryDirectory() as directory:
            result = subprocess.run(
                [sys.executable, str(Path(__file__).with_name('trace-public-api-compiler.py')),
                 '--project', 'Azure', '--trace-tool', '/unused', '--output-directory', directory,
                 '--', sys.executable, '-c', 'raise SystemExit(7)'], timeout=10)
            self.assertEqual(7, result.returncode)

    @unittest.skipUnless(platform.system() == 'Linux', 'Process group cleanup is Linux-only.')
    def test_cleanup_terminates_the_owned_process_group(self):
        process = subprocess.Popen([sys.executable, '-c', 'import time; time.sleep(60)'],
                                   start_new_session=True)
        try:
            self.assertEqual(process.pid, os.getpgid(process.pid))
            compiler_trace.stop_owned_process(process)
            self.assertIsNotNone(process.poll())
        finally:
            if process.poll() is None:
                process.kill()
                process.wait()


if __name__ == '__main__':
    unittest.main()
