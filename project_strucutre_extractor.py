def list_folder_structure(parent_folder, file_extensions=None, exclude_folders=None):
    import os

    if file_extensions is None:
        file_extensions = ['.md', '.py', 'Dockerfile', '.json', '.txt', '.cs', '.csproj']

    if exclude_folders is None:
        exclude_folders = ['.idea', '__pycache__']

    for root, dirs, files in os.walk(parent_folder, topdown=True):
        dirs[:] = [d for d in dirs if d not in exclude_folders]
        level = root.replace(parent_folder, '').count(os.sep)
        indent = ' ' * 4 * (level)
        print(f'{indent}{os.path.basename(root)}/')
        subindent = ' ' * 4 * (level + 1)
        for f in files:
            if any(f.endswith(ext) or f == ext for ext in file_extensions):
                print(f'{subindent}{f}')


# Example usage
list_folder_structure('D:\IliasTsichlis\Bank.API')