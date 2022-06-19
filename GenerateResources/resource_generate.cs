using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


var pattern = "*filled*.svg";
var resourceFilePath = "Properties";
var resourceFileName = "FluentIconsFilled";

var resxFileEntry = @"
<data name=""{0}"" type=""System.Resources.ResXFileRef, System.Windows.Forms"">
    <value>{1};System.Byte[], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
  </data>
";

var resxDesignerEntry = @"
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

Directory.CreateDirectory(resourceFilePath);

var files = GetFilesForResource("../fluentui-system-icons/assets", pattern).ToList();

Console.WriteLine($"Files found: {files.Count}...");

foreach (var file in files)
{
    var fileName = Path.GetFileName(file);
    var destinationFile = Path.Combine(resourceFilePath, fileName);
    Console.WriteLine($"Copy file: '{file}' --> '{destinationFile}'.");
    File.Copy(file, destinationFile, true);
}

var resxFileName = resourceFileName + ".resx";
var resxDesignerFileName = resourceFileName + ".Designer.cs";
var resxFile = File.CreateText(resxFileName);
var resxDesignerFile = File.CreateText(resxDesignerFileName);

resxFile.Write(File.ReadAllText("resx_file_start.txt"));
resxDesignerFile.Write(File.ReadAllText("empty_resources_template.txt"));



foreach (var file in files)
{
    var fileName = Path.GetFileName(file);
    var destinationFile = Path.Combine(resourceFilePath, fileName);

    resxFile.Write(
        string.Format(resxFileEntry, 
            Path.GetFileNameWithoutExtension(file), 
            $@".\{destinationFile}"));

    resxDesignerFile.Write(
        resxDesignerEntry.Replace("#NAME#", Path.GetFileNameWithoutExtension(file)));
}

resxDesignerFile.Write("    }");
resxDesignerFile.Write("}");

resxFile.WriteLine("</root>");
resxFile.Flush();
resxDesignerFile.Flush();
resxFile.Dispose();
resxDesignerFile.Dispose();

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