# Virtual Disk Manager Console Application

Welcome to the Virtual Disk Manager Console Application! This simple and intuitive application allows you to manage a virtual disk through a variety of commands, similar to a traditional command-line interface.

## Features

- `help`:   Display information about the commands
- `cls`:    Clear the Console
- `quit`:   Exit the Console
- `cd`:     Change the current default directory
- `copy`:   Copies one or more files to another location
- `del`:    Deletes one or more files
- `md`:     Creates a directory
- `rd`:     Remove directory
- `rename`: Renames a file
- `type`:   Displays the contents of a text file
- `import`: Import text file from your computer
- `export`: Export text file to your computer

## Getting Started

Follow these instructions to get a copy of the project up and running on your local machine.

### Prerequisites

- .NET Core SDK

### Installation

1. Clone the repository:
    ```sh
    git clone https://github.com/Elaraby218/virtual-disk-manager.git
    ```
2. Navigate to the project directory:
    ```sh
    cd virtual-disk-manager
    ```
3. Build the project:
    ```sh
    dotnet build
    ```
4. Run the application:
    ```sh
    dotnet run
    ```

## Usage

Once the application is running, you can use the following commands:

### Commands that Manage the Console

1. `help`   - Displays information about the available commands.
2. `cls`    - Clears the console screen.
3. `quit`   - Exits the console application.

### Commands that Manage Directories

1. `cd` - Changes the current directory. Usage: `cd [directory_name]`
2. `md` - Creates a new directory. Usage: `md [directory_name]`
3. `rd` - Removes a directory. Usage: `rd [directory_name]`
4. `rename` - Renames a directory or file. Usage: `rename [old_name] [new_name]`
5. `copy` - Copies files from one location to another. Usage: `copy [source_file] [destination_file]`

### Commands that Manage Files

1. `import` - Imports a text file from your computer into the virtual disk. Usage: `import [file_path]`
2. `export` - Exports a text file from the virtual disk to your computer. Usage: `export [file_name] [destination_path]`
3. `type` - Displays the contents of a text file. Usage: `type [file_name]`
4. `del` - Deletes specified files. Usage: `del [file_name]`

## Contributing

Contributions are welcome! Please fork this repository and submit a pull request with your changes. For major changes, please open an issue first to discuss what you would like to change.

## Contact

If you have any questions or feedback, feel free to reach out to me at [LinkedIn]([mailto:your-email@example.com](https://www.linkedin.com/in/mohammed-ramadan-elaraby-097245242/)).

---

Made by [Mohamed Elaraby](https://github.com/Elaraby218)
