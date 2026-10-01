using System.Collections.Concurrent;
using System.Text.RegularExpressions;

var files = Directory.GetFiles("Files", "*.txt");
var results = new ConcurrentDictionary<string, FileStatistics>();

Parallel.ForEach(files, file =>
{
    results[Path.GetFileName(file)] = fileStatistics(file);
});

foreach (var kvp in results)
{
    PrintStatistics(kvp.Key, kvp.Value);
}

PrintTotalStatistics(results);

static void PrintStatistics(string filePath, FileStatistics statistics)
{
    Console.WriteLine();
    Console.WriteLine($"File:       {filePath}");
    Console.WriteLine($"Characters: {statistics.Characters}");
    Console.WriteLine($"Words:      {statistics.Words}");
    Console.WriteLine($"Lines:      {statistics.Lines}");
    Console.WriteLine($"Errors:     {statistics.Errors}");
    Console.WriteLine();
}

static void PrintTotalStatistics(ConcurrentDictionary<string, FileStatistics> results)
{
    Console.WriteLine("===== TOTAL =====");
    Console.WriteLine();
    Console.WriteLine($"Files:      {results.Count}");
    Console.WriteLine($"Characters: {results.Values.Sum(s => s.Characters)}");
    Console.WriteLine($"Words:      {results.Values.Sum(s => s.Words)}");
    Console.WriteLine($"Lines:      {results.Values.Sum(s => s.Lines)}");
    Console.WriteLine($"Errors:     {results.Values.Sum(s => s.Errors)}");
}

static FileStatistics fileStatistics (string filePath)
{
    string text = File.ReadAllText(filePath);
    int lines = 0;
    using (var reader = new StringReader(text))
    {
        
        while (reader.ReadLine() != null)
        {
            lines++;
        }
    }

    return new FileStatistics
    {
        Characters = text.Length,
        Words = text.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries).Length,
        Lines = lines,
        Errors = Regex.Matches(text, "error", RegexOptions.IgnoreCase).Count
    };
}
;


