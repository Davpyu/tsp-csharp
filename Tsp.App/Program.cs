using Tsp.App.Services;

List<string> buildings = ["Data Center", "Kantor 1", "Kantor 2", "Pemukiman 1", "Pemukiman 2"];
Dictionary<(string, string), int> distances = new()
{
    { ("Data Center", "Kantor 1"), 30 }, { ("Data Center", "Kantor 2"), 20 }, { ("Data Center", "Pemukiman 1"), 10 },
    { ("Kantor 1", "Pemukiman 1"), 5 },  { ("Kantor 1", "Pemukiman 2"), 25 },
    { ("Kantor 2", "Pemukiman 1"), 13 }, { ("Kantor 2", "Pemukiman 2"), 40 },
    { ("Kantor 1", "Data Center"), 30 }, { ("Kantor 2", "Data Center"), 20 }, { ("Pemukiman 1", "Data Center"), 10 },
    { ("Pemukiman 1", "Kantor 1"), 5 },  { ("Pemukiman 2", "Kantor 1"), 25 },
    { ("Pemukiman 1", "Kantor 2"), 13 }, { ("Pemukiman 2", "Kantor 2"), 40 }
};

Console.WriteLine("Available Paths:");

foreach (KeyValuePair<(string, string), int> item in distances)
{
    Console.WriteLine($"Path: {item.Key.Item1} -> {item.Key.Item2}, Distance: {item.Value} KM");
}

TspService tspService = new("Data Center", buildings, distances);

List<(List<string>, int)> result = tspService.GetPaths();

Console.WriteLine("Result:");
foreach ((List<string>, int) path in result)
{
    Console.WriteLine($"Path: {string.Join(" -> ", path.Item1)} Distance: {path.Item2} KM");
}

Console.WriteLine($"Total length: {result.Sum(x => x.Item2)}KM");