using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

int[] sizes = {16, 20, 24, 28, 32, 48,};
string[] sizeTypes = { "Filled", "Regular", };

string resXFileEntry = @"
<data name=""{0}"" type=""System.Resources.ResXFileRef, System.Windows.Forms"">
    <value>{1};System.Byte[], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </data>
";

string resXDesignerEntry = @"
        /// <summary>
        ///   Looks up a localized resource of type System.Byte[].
        /// </summary>
        public static byte[] #NAME# {
            get {
                object obj = ResourceManager.GetObject(""#NAME#"", resourceCulture);
                return ((byte[])(obj));
            }
        }
";

var destinationDirectory = "../FluentIcons.Resources/Properties";
var destinationResXDirectory = "../FluentIcons.Resources";
Directory.CreateDirectory(destinationDirectory);

foreach(var sizeType in sizeTypes)
{
    foreach (var size in sizes)
    {
        var pattern = $"ic_fluent*{size}*{sizeType.ToLower()}*.svg";
        Console.WriteLine($"Search pattern '{pattern}'.");
        var files = GetFilesForResource("../../fluentui-system-icons/assets", pattern).ToList();
        Console.WriteLine($"{files.Count}");
        var destinationFolder = Path.Combine(destinationResXDirectory);
        var destinationFileResX = Path.Combine(destinationFolder, sizeType, $"Size{size}.resx");
        var destinationFileCs = Path.Combine(destinationFolder, sizeType, $"Size{size}.Designer.cs");
        Console.WriteLine($"Create file: '{destinationFileResX}'...");
        var resXFile = File.CreateText(destinationFileResX);
        Console.WriteLine($"Create file: '{destinationFileCs}'...");
        var resXDesignerFile = File.CreateText(destinationFileCs);
        resXFile.Write(File.ReadAllText("resx_file_start.txt"));
        resXDesignerFile.Write(File.ReadAllText("empty_resources_template.txt")
            .Replace("#CLASSNAME#", $"public class {sizeType}{size}")
            .Replace("#CONSTRUCTOR#", $"{sizeType}{size}")
            .Replace("#NAMESPACE.CLASS#", $"FluentIcons.Resources.{sizeType}{size}")
            .Replace("#NAMESPACE#", $"FluentIcons.Resources.{sizeType}"));

        var addedResources = new List<string>();

        foreach (var file in files)
        {
            var fileName = Path.GetFileName(file);

            if (addedResources.Contains(fileName)) 
            {
                continue;
            }

            addedResources.Add(fileName);

            var destinationFile = Path.Combine(destinationDirectory, fileName);
            Console.WriteLine($"Copy file '{file}' --> '{destinationFile}'.");
            File.Copy(file, destinationFile, true);

            resXFile.Write(
                string.Format(resXFileEntry, 
                    Path.GetFileNameWithoutExtension(fileName), 
                    $@"..\Properties\{fileName}"));

            resXDesignerFile.Write(
                resXDesignerEntry.Replace("#NAME#", Path.GetFileNameWithoutExtension(file)));            
        }

        resXDesignerFile.WriteLine("    }");
        resXDesignerFile.WriteLine("}");

        resXFile.WriteLine("</root>");

        resXFile.Flush();
        resXDesignerFile.Flush();
        resXFile.Dispose();
        resXDesignerFile.Dispose();
    }
}

string user()
    => Environment.UserName;

IEnumerable<string> GetFilesForResource(string path, string searchPattern)
{
    var result = new List<string>();
    var files = Directory.GetFiles(path, searchPattern);

    foreach (var subPath in Directory.GetDirectories(path))
    {
        try 
        {
            result.AddRange(GetFilesForResource(subPath, searchPattern));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: '{ex.Message}'.");
        }
    }

    result.AddRange(files);

    return result;
}