#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
using System;

namespace aurw.develop
{
    public class AddHLSLFile
    {
        private static string defaultContent = "// Start making your HLSL";

        [MenuItem("Assets/Create/Shader/HLSL File")]
        [MenuItem("Assets/Create/Shader Graph/HLSL File")]
        public static void ShowWindow()
        {
            string folderPath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
            string fullFolderPath = Path.GetFullPath(folderPath);

            string baseName = "New HLSL File";
            string extension = ".hlsl";
            string pattern = $@"^{baseName}(\s\d+)?{extension}$";

            // Find all existing files with similar names
            string[] existingFiles = Directory.GetFiles(fullFolderPath, $"{baseName}*{extension}");

            int highestNumber = 0;
            Regex regex = new Regex(pattern);

            foreach (string file in existingFiles)
            {
                string fileName = Path.GetFileName(file);
                if (regex.IsMatch(fileName))
                {
                    Match match = Regex.Match(fileName, $@"{baseName}\s(?<number>\d+){extension}");
                    if (match.Success && int.TryParse(match.Groups["number"].Value, out int number))
                    {
                        if (number > highestNumber)
                            highestNumber = number;
                    }
                    else
                    {
                        // This is the base file without a number
                        highestNumber = Math.Max(highestNumber, 1);
                    }
                }
            }

            string newFileName = highestNumber == 0
                ? $"{baseName}{extension}"
                : $"{baseName} {highestNumber + 1}{extension}";

            string fullPath = Path.Combine(fullFolderPath, newFileName);

            using (StreamWriter sw = File.CreateText(fullPath))
            {
                sw.WriteLine(defaultContent);
            }

            AssetDatabase.Refresh();

            // Select the newly created file
            string relativePath = Path.Combine(folderPath, newFileName).Replace("\\", "/");
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath(relativePath, typeof(UnityEngine.Object));
        }
    }
}
#endif