# Roadmap
## V.1
### Bugs
- Validation missing (everywhere!)
- Assessment result rows change order on validation failure
- Virtically align the data entry boxes on the assessment editor & listings so top and bottom spacing to line is equal

### Improvements
- Add confirmations of destructive operations
- Draw more attention to the standard "+New" button
- Make new assessment window modal (if JS enabled) & remove cancel button
- Make grade boundaries model (if JS enabled)
- The button on grade boundaries should probably read "done". This saves everything and closes the modal, returning you to the main results entry page.
- Is the "save changes" button redundant? It appears to save changes anyway - I guess that is what the tool tip is saying. But this means that you can remove it.
- I think you could probably remove the "add new candidate" button. When you get to the end of the row (i.e. entering the mark), it should automatically add in a new row, but greyed out. When you hit return for the mark, it takes you to the next row.

### New Features
- Add user email address plus validation during registration
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
