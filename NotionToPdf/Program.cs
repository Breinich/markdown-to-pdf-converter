using System.Diagnostics;
using System.IO.Compression;

Console.WriteLine("Enter the absolute path of the generated .zip file containing the markdown document: ");

var workDir = "workdir";
var path = Console.ReadLine();

ZipFile.ExtractToDirectory(path ?? throw new ArgumentException("The given path is empty."), 
    workDir);

var docs = Directory.GetFiles(workDir, "*.md");
if (docs.Length == 0)
    throw new ApplicationException("There isn't any document to convert.");

var outWriter = File.AppendText("out.txt");
var errWriter = File.AppendText("err.txt");

foreach (var doc in docs)
{
    outWriter.WriteLine($"{doc}:");
    
    var conversion = new Process
    {
        StartInfo =
        {
            FileName = "pandoc",
            Arguments = $"\"{doc}\" -o \"{Path.ChangeExtension(doc, "pdf")}\" -F mermaid-filter",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        }
    };

    conversion.Start();

    outWriter.WriteLine(conversion.StandardOutput.ReadToEnd());
    errWriter.WriteLine(conversion.StandardError.ReadToEnd());

    conversion.WaitForExit();
}

outWriter.Close();
errWriter.Close();