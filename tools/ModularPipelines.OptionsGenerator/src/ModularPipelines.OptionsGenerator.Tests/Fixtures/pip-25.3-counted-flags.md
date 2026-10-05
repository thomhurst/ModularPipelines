# Pip 25.3 counted general options

The captured root help identifies `--verbose` / `-v` and `--quiet` / `-q` as
additive flags. The upstream [option registrations](https://github.com/pypa/pip/blob/25.3/src/pip/_internal/cli/cmdoptions.py)
use optparse `action="count"` with a zero default for both flags.

The installed pip 25.3 parser was invoked without executing a command:

```python
from pip._internal.cli.main_parser import create_main_parser
options, arguments = create_main_parser().parse_args(
    ["--verbose"] * 4 + ["--quiet"] * 3
)
assert options.verbose == 4
assert options.quiet == 3
```

Help describes three useful levels, but the parser does not impose a maximum
count. Generated nullable integer flags therefore require nonnegative counts
without a maximum of three. Zero and null omit the switch. Root aliases retain
their case: verbosity uses `-v`, while the version action uses `-V`.
