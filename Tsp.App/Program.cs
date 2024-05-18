using Tsp.App.Services;

List<string> buildings = ["Data Center", "Kantor 1", "Kantor 2", "Pemukiman 1", "Pemukiman 2"];
List<Tsp.App.Models.Path> distances =
[
    new() { From = "Data Center", To = "Kantor 1", Length = 30 },
    new() { From = "Data Center", To = "Kantor 2", Length = 20 },
    new() { From = "Data Center", To = "Pemukiman 1", Length = 10 },
    new() { From = "Kantor 1", To = "Pemukiman 1", Length = 5 },
    new() { From = "Kantor 1", To = "Pemukiman 2", Length = 25 },
    new() { From = "Kantor 2", To = "Pemukiman 1", Length = 13 },
    new() { From = "Kantor 2", To = "Pemukiman 2", Length = 40 },
    new() { From = "Kantor 1", To = "Data Center", Length = 30 },
    new() { From = "Kantor 2", To = "Data Center", Length = 20 },
    new() { From = "Pemukiman 1", To = "Data Center", Length = 10 },
    new() { From = "Pemukiman 1", To = "Kantor 1", Length = 5 },
    new() { From = "Pemukiman 2", To = "Kantor 1", Length = 25 },
    new() { From = "Pemukiman 1", To = "Kantor 2", Length = 13 },
    new() { From = "Pemukiman 2", To = "Kantor 2", Length = 40 }
];

Console.WriteLine("Available Paths:");

foreach (Tsp.App.Models.Path item in distances)
{
    Console.WriteLine($"Path: {item.From} -> {item.To}, Distance: {item.Length} KM");
}

TspService tspService = new("Data Center", buildings, distances);

List<(List<string>, int)> result = tspService.GetPaths();

Console.WriteLine("Result:");
foreach ((List<string>, int) path in result)
{
    Console.WriteLine($"Path: {string.Join(" -> ", path.Item1)}, Distance: {path.Item2} KM");
}

Console.WriteLine($"Total distance: {result.Sum(x => x.Item2)}KM");