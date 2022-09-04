# Simple-C-GitHub-API

Example
<code>
    GitHub.UserAgent = "Test by (your github username here)";
    
    foreach (var file in GitHub.Files("ComputerKim", "ParallelForEach.ps1")) {
        Console.WriteLine($"{file.path} {file.url} {GitHub.File(file.url)}");
    }
    
    var zip = GitHub.Zip("ComputerKim", "ParallelForEach.ps1");
    Console.WriteLine($"Zip {zip}");
    
    Console.WriteLine("Done.");
    Console.ReadLine();
</code>