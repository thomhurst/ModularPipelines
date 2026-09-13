Edit one or more issues within the same repository.

Editing issues' projects requires authorization with the `project` scope.
To authorize, run `gh auth refresh -s project`.

Use `--attach` to upload an image or video to a single issue. Without a body
flag the issue keeps the body it already has and the attachment is appended to it.
If the body references an attached file, such as `![alt](./login.png)`, that
reference is rewritten to point at the uploaded asset instead.
You can attach up to 50 files per command.

Alt text for an image follows the path after `#`, as in
`--attach './login.png#The login error state'`. Without it the filename is used.
A reference already in the body keeps the alt text written there. Video renders
as a player and has no alt text, so it cannot be given any.

If some attachments upload and others fail, the issue is still updated with the
ones that succeeded. The command then exits with a non-zero status, but the edited
issue URLs are still printed to stdout.

The `--add-assignee` and `--remove-assignee` flags both support
the following special values:
- `@me`: assign or unassign yourself
- `@copilot`: assign or unassign Copilot (not supported on GitHub Enterprise Server)


USAGE
  gh issue edit {<numbers> | <urls>} [flags]

FLAGS
      --add-assignee login         Add assigned users by their login. Use "@me" to assign yourself, or "@copilot" to assign Copilot.
      --add-blocked-by number      Add 'blocked by' relationships by issue number or URL
      --add-blocking number        Add 'blocking' relationships by issue number or URL
      --add-label name             Add labels by name
      --add-project title          Add the issue to projects by title
      --add-sub-issue number       Add sub-issues by number or URL
      --attach file                Attach an image or video file, in '<file>#<image alt text>' format
  -b, --body string                Set the new body.
  -F, --body-file file             Read body text from file (use "-" to read from standard input)
  -m, --milestone name             Edit the milestone the issue belongs to by name
      --parent number              Set the parent issue by number or URL
      --remove-assignee login      Remove assigned users by their login. Use "@me" to unassign yourself, or "@copilot" to unassign Copilot.
      --remove-blocked-by number   Remove 'blocked by' relationships by issue number or URL
      --remove-blocking number     Remove 'blocking' relationships by issue number or URL
      --remove-label name          Remove labels by name
      --remove-milestone           Remove the milestone association from the issue
      --remove-parent              Remove the parent issue
      --remove-project title       Remove the issue from projects by title
      --remove-sub-issue number    Remove sub-issues by number or URL
      --remove-type                Remove the issue type from the issue
  -t, --title string               Set the new title.
      --type name                  Set the issue type by name

INHERITED FLAGS
      --help                     Show help for command
  -R, --repo [HOST/]OWNER/REPO   Select another repository using the [HOST/]OWNER/REPO format

EXAMPLES
  $ gh issue edit 23 --title "I found a bug" --body "Nothing works"
  $ gh issue edit 23 --add-label "bug,help wanted" --remove-label "core"
  $ gh issue edit 23 --add-assignee "@me" --remove-assignee monalisa,hubot
  $ gh issue edit 23 --add-assignee "@copilot"
  $ gh issue edit 23 --add-project "Roadmap" --remove-project v1,v2
  $ gh issue edit 23 --milestone "Version 1"
  $ gh issue edit 23 --remove-milestone
  $ gh issue edit 23 --body-file body.txt
  $ gh issue edit 23 --attach './login.png#The login error state'
  $ gh issue edit 23 --attach ./before.png --attach ./after.png
  $ gh issue edit 23 34 --add-label "help wanted"
  $ gh issue edit 23 --type Bug
  $ gh issue edit 23 --remove-type
  $ gh issue edit 23 --parent 100
  $ gh issue edit 23 --remove-parent
  $ gh issue edit 100 --add-sub-issue 123,124
  $ gh issue edit 123 --add-blocked-by 200 --add-blocking 300,301

LEARN MORE
  Use `gh <command> <subcommand> --help` for more information about a command.
  Read the manual at https://cli.github.com/manual
  Learn about exit codes using `gh help exit-codes`
  Learn about accessibility experiences using `gh help accessibility`
