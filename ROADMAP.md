# Project Roadmap: Steam Games Branch Manager

This roadmap outlines the planned improvements and features for the **Steam Games Branch Manager** project. Tasks are categorized by priority and include descriptions, steps, and statuses.

---

## **High Priority**

These tasks are critical for improving the functionality, usability, and stability of the application.

### 1. **Unmanaged Games Detection**

- **Description**: Enhance the application's ability to detect installed Steam games automatically by parsing local Steam files.
- **Status**: **In Progress**.
- **Steps**:
  - Parse the `libraryfolders.vdf` file in the Steam installation directory to locate all Steam libraries.
  - Scan each library for installed games by reading `.acf` files in the `steamapps` folder.
  - Automatically detect the game name from the `.acf` file, removing the need for manual naming.
  - Populate the game list dynamically based on detected games.
- **Fallback Option**:
  - The existing fallback system allows users to manually add a file path if detection fails.

### 2. **Modernize UI Layout**

- **Description**: Update the UI to be more dynamic and visually appealing.
- **Status**: **Partially Completed**.
- **Steps**:
  - Used `TableLayoutPanel` for dynamic resizing.
  - Added spacing and padding between controls to reduce clutter.
  - Next Steps:
    - Add icons to buttons and menu items for better visual clarity.
    - Improve the overall visual design with modern styling.

---

## **Medium Priority**

These tasks improve the user experience and add new features to the application.

### 3. **Add Progress Feedback**

- **Description**: Provide visual feedback for long-running operations like creating or deleting branches.
- **Status**: **Not Started**.
- **Steps**:
  - Add a `ProgressBar` to the bottom of the main window.
  - Update the progress bar during `BackgroundWorker` operations (e.g., file copying, branch creation).
  - Display status messages in a `StatusStrip` or label.

### 4. **Introduce Game Details Panel**

- **Description**: Add a panel to display detailed information about the selected game.
- **Status**: **Not Started**.
- **Steps**:
  - Show game name, installation path, and available branches.
  - Include a preview of the `.acf` file contents for advanced users.
  - Allow editing of game metadata (e.g., custom display name).

### 5. **Add Context Menus**

- **Description**: Add right-click context menus for `Games` and `Branches` lists.
- **Status**: **Not Started**.
- **Steps**:
  - For `Games`, include options like "Add Game", "Rename Game", "Delete Game".
  - For `Branches`, include options like "Set Active", "Rename Branch", "Delete Branch".

---

## **Low Priority**

These tasks are enhancements that improve the overall polish and usability of the application.

### 6. **Add Theming Support**

- **Description**: Allow users to switch between light and dark themes.
- **Status**: **Not Started**.
- **Steps**:
  - Use a theming library or custom styles to implement themes.
  - Add a "Theme" option in the menu to toggle between themes.

### 7. **Add Dashboard View**

- **Description**: Create a dashboard-like interface to display summary information.
- **Status**: **Not Started**.
- **Steps**:
  - Show total games, total branches, and last modified branch.
  - Use cards or tiles to display key actions and statistics.
  - maybe a history leaderboard with fastest copy high scores calculated by total copy duration / total size copied so a fast copy but low size equals out with a HUGE copy over a longer time and rank them overall with date and time and total size and total time.

### 8. **Accessibility Enhancements**

- **Description**: Improve accessibility for users with disabilities.
- **Status**: **Not Started**.
- **Steps**:
  - Add tooltips to all buttons and controls.
  - Ensure all controls have accessible names for screen readers.
  - Add keyboard shortcuts for common actions (e.g., `Ctrl+N` for adding a game).

---

## **New Tasks**

### 9. **Improve Error Handling**

- **Description**: Add robust error handling for file operations and symbolic link creation.
- **Status**: **Not Started**.
- **Steps**:
  - Display detailed error messages when operations fail.
  - Add retry mechanisms for recoverable errors.
  - Log errors to a file for debugging purposes.

### 10. **Add Logging**

- **Description**: Implement logging to track application events and errors.
- **Status**: **Not Started**.
- **Steps**:
  - Use a logging library like `Serilog` or `NLog`.
  - Log events such as game detection, branch creation, and errors.
  - Provide an option to view logs in the application.

### 11. **Implement Unit Tests**

- **Description**: Add unit tests to ensure the reliability of critical components.
- **Status**: **Not Started**.
- **Steps**:
  - Set up a testing framework such as `xUnit` or `NUnit`.
  - Write unit tests for critical components like `BranchHandler` and `SaveData`.
  - Mock dependencies to isolate test cases.
  - Add continuous integration (CI) to run tests automatically on commits.
  - Ensure code coverage metrics are tracked and improved over time.

---

## **Proposed Timeline**

| Priority   | Task                         | Estimated Completion |
| ---------- | ---------------------------- | -------------------- |
| **High**   | Unmanaged Games Detection    | 3-5 days             |
| **High**   | Modernize UI Layout          | 2-3 days             |
| **Medium** | Add Progress Feedback        | 2-3 days             |
| **Medium** | Introduce Game Details Panel | 3-4 days             |
| **Medium** | Add Context Menus            | 2-3 days             |
| **Low**    | Add Theming Support          | 3-5 days             |
| **Low**    | Add Dashboard View           | 3-5 days             |
| **Low**    | Accessibility Enhancements   | 2-3 days             |
| **Low**    | Improve Error Handling       | 2-3 days             |
| **Low**    | Add Logging                  | 2-3 days             |
| **Low**    | Implement Unit Tests         | 3-5 days             |

---

## **Next Steps**

1. **Continue with High Priority Tasks**:

   - Complete the implementation of automatic game detection, including parsing `.acf` files and removing the manual naming requirement.
   - Finalize the modernization of the UI layout.

2. **Start Medium Priority Tasks**:

   - Add progress feedback and a game details panel to enhance the user experience.

3. **Plan for Low Priority Tasks**:
   - Add theming, a dashboard view, and accessibility features to modernize the application.
   - Implement error handling and logging for better stability and debugging.
   - Begin writing unit tests to ensure code reliability.

This roadmap provides a clear path forward for improving the **Steam Games Branch Manager**. Mark tasks as completed as progress is made.
