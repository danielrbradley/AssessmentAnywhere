# Roadmap
## V.1
### Bugs
- Validation missing (everywhere!)
- Assessment result rows change order on validation failure

### Improvements
- Use IoC container for dependency injection
- Add confirmations of destructive operations
- Draw more attention to the standard "+New" button
- Add modal windows if JS is enabled:
  - Make new assessment window modal & remove cancel button
  - Make grade boundaries modal and have a single "done" buttom which saves everything and closes the modal
- Automatically save changes in the assessment editor after changing any cell.
- Show new add candidate row after entering the forename and surname of a new candidate.

### New Features
- Persistent storage of data

### Optional
- Import CSV/XLS
- Export CSV/XLS
- Copy names from another assessment
- Copy boundaries from another assessment (or by template)
- Add AJAX MVVM model to assessment editor

## V.2
- Assessment tags
- Reports generation ...
  - Select multiple assessments (by tag or manually)
  - Extract grades/percentages or define new grade boundaries

## V.next
- User sharing of assessments
- Undo/redo of all actions
- Custom candidate ordering
- Column re-ordering (limited to percentage & grade?)
