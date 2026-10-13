# pip 26.2.1 help fixtures

Captured from an isolated pip 26.2.1 installation with `pip --help` and
`pip <command> --help` for install, download, wheel, lock, index, and list.
Trailing line whitespace is normalized; option indentation and wrapped text are preserved.
The older pip 25.3 fixtures remain as compatibility regressions.

The help does not describe all repeatable inputs. In the
[version-pinned parser source](https://github.com/pypa/pip/blob/26.2.1/src/pip/_internal/cli/cmdoptions.py):

- `requirements_from_scripts` uses `action="append"`.
- `_handle_refresh_package` accumulates repeated values and processes comma-delimited
  package names and reset markers. Each supplied string must remain
  one switch operand; the generator must not split or reorder values.
- `no_proxy_env` is a root `store_true` flag in the General Options group.

The requirement-command usage summaries omit standalone script inputs, just as
they omit standalone dependency groups. The scraper supplements these forms so
script inputs alone satisfy generated input validation.

`extra_index_url` and `find_links` also use `action="append"`, even though their
help omits repeatability. They remain command-specific Package Index Options.
The fixtures were rechecked against pip 26.2.1 on Python 3.14.3 with `COLUMNS=80`.
All 15 functional commands are supported, including `lock`; `completion`, `debug`,
and `help` remain excluded utility commands.

`editable` and `list_exclude` use `action="append"` without help declaring
repeatability. Editable projects can serve as the complete input source, and
freeze/list exclusions retain every supplied package name.
