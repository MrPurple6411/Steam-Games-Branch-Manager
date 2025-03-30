# Steam Games Branch Manager

Steam Games Branch Manager is a Windows desktop application designed to help users manage multiple versions (branches) of their Steam games. It allows users to create, switch between, rename, and delete branches of their games, providing a convenient way to manage game modifications or different versions.

## Features

- **Game Management**:

  - Add Steam games manually by selecting their installation folder and `.acf` file.
  - Automatically save and load game data for future sessions.

- **Branch Management**:

  - Create branches of games, including an "original" branch for backups.
  - Switch between branches using symbolic links.
  - Rename or delete branches as needed.

- **UI Features**:

  - Dynamic UI layout using `TableLayoutPanel` for better resizing.
  - Labels and lists for displaying games and branches.
  - Background workers for long-running tasks like branch creation and deletion.

- **Error Handling**:

  - Retry mechanisms for branch creation and deletion.
  - User-friendly error messages for common issues.

- **Administrative Privileges**:
  - Requires administrator privileges to create symbolic links for branch switching.

## Requirements

- **Operating System**: Windows 10 or later.
- **Framework**: .NET 9.0 Runtime.
- **Steam**: Installed Steam client with games installed.
- **Privileges**: Administrator rights are required to create symbolic links.

## Installation

1. Download the latest release from the [Releases](https://github.com/MrPurple6411/Steam-Games-Branch-Manager/releases) page.
2. Extract the downloaded files to a folder of your choice.
3. Run `Steam Games Branch Manager.exe` as an administrator.

## Usage

1. **Adding a Game**:

   - Insert Games Name in text box (Soon to be not needed)
   - Click the "Add Game" button.
   - Select the game's installation folder and `.acf` file from the Steam `steamapps` directory.

2. **Creating a Branch**:

   - Select a game from the list.
   - Enter a branch name in the text box.
   - Click "Create Branch" to create a new branch.

3. **Switching Branches**:

   - Select a branch from the list and check it's box to make it active.
   - The application will use symbolic links to switch to the selected branch.

4. **Renaming or Deleting**:

   - Use the "Rename" or "Delete" buttons to modify or remove games or branches.

5. **Viewing About Information**:
   - Access the "Help" menu and click "About" to view application details.

## Development

### Prerequisites

- Visual Studio Code or Visual Studio 2022.
- .NET 9.0 SDK.
- Administrator privileges for debugging symbolic link operations.

### Building the Project

1. Clone the repository:

   ```bash
   git clone https://github.com/MrPurple6411/Steam-Games-Branch-Manager.git
   cd Steam-Games-Branch-Manager
   ```

2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Build the project:

   ```bash
   dotnet build --configuration Release
   ```

4. Run the application:
   ```bash
   dotnet run --project "Steam Games Branch Manager.csproj"
   ```

### Project Structure

- **MainForm.cs**: The main UI logic for managing games and branches.
- **BranchHandler.cs**: Handles branch creation, deletion, and symbolic link operations.
- **SaveData.cs**: Manages the application's saved data, including games and branches.
- **Program.cs**: Entry point for the application, ensuring administrator privileges.
- **Resources**: Contains UI resources and settings.

### Key Dependencies

- [Newtonsoft.Json](https://www.newtonsoft.com/json): For JSON serialization and deserialization.
- [Ookii.Dialogs.WinForms](https://github.com/ookii-dialogs/ookii-dialogs-winforms): For modern file and folder dialogs.

## Troubleshooting

- **Administrator Privileges**:

  - Ensure the application is run as an administrator to create symbolic links.

- **Missing Dependencies**:

  - Install the .NET 9.0 Runtime if not already installed.

- **Steam Game Detection**:
  - Ensure the selected folder and `.acf` file match the game's installation.

## Contributing

Contributions are welcome! Please submit issues or pull requests on the [GitHub repository](https://github.com/MrPurple6411/Steam-Games-Branch-Manager).

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.

## Acknowledgments

- **Newtonsoft.Json**: JSON serialization library.
- **Ookii.Dialogs.WinForms**: Modern dialog library for Windows Forms.
