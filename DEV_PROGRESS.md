# Steam Games Branch Manager - Development Progress Tracker

## Overview
This document tracks the progress, ideas, and tasks for both the legacy WinForms app and the new cross-platform UI project. Use this as a central place to check off completed work, add new ideas, and keep context for GitHub Copilot Agent or any developer.

---

## Current Projects
- **Legacy:** Steam Games Branch Manager (WinForms, .NET Framework 4.7.2)
- **Next Gen:** [Planned] Cross-platform UI (Avalonia UI or MAUI)

---

## Phase 1: Quick WinForms Improvements
- [ ] Improve layout with TableLayoutPanel/FlowLayoutPanel
- [ ] Add padding/margins to controls
- [ ] Increase font sizes and use bold/colored labels
- [ ] Add icons to buttons (optional)
- [ ] Integrate a WinForms theme library (e.g., MaterialSkin)
- [ ] Add tooltips to buttons
- [ ] Make window resizable and controls anchor/stretch
- [ ] Add confirmation dialogs for destructive actions
- [ ] Group related controls in GroupBoxes
- [ ] Refactor control names for clarity

---

## Phase 2: New Cross-Platform Project
- [ ] Add new Avalonia UI project to the solution
- [ ] Create a shared core library for business logic and data models
- [ ] Move business logic from WinForms to shared core
- [ ] Design modern UI in Avalonia (responsive layout, theming, icons)
- [ ] Implement feature parity with WinForms app
- [ ] Add new features (search, drag-and-drop, etc.)
- [ ] Test on Windows, Linux, macOS
- [ ] Gradually migrate users to new app

---

## Phase 3: Maintenance & Transition
- [ ] Maintain both apps in solution
- [ ] Encourage feedback on new app
- [ ] Deprecate WinForms app when ready

---

## Ideas & Feature Requests
- [ ] Add search/filter for games and branches
- [ ] Add branch description/notes
- [ ] Add dark mode toggle
- [ ] Add backup/restore for save.json
- [ ] Add error reporting/log viewer UI
- [ ] Add drag-and-drop for game folders
- [ ] Add multi-language support
- [ ] Add update checker

---

## Bugs & Fixes
- [ ] Ensure all file/folder operations use installdir, not display name
- [ ] Improve error messages for file/folder issues
- [ ] Handle symlink edge cases more gracefully

---

## How to Use
- Mark tasks as `[x]` when complete.
- Add new ideas, bugs, or requests to the relevant section.
- Reference this file in Copilot Agent conversations to maintain context.

---

_Last updated: [Copilot Agent will update as needed]_
