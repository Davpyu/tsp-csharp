namespace Tsp.App.Services;

public class TspService
{
    public static List<List<string>> Run(string start, List<string> buildings, Dictionary<(string, string), int> distances)
    {
        List<List<string>> shortestPaths = [];

        foreach (string building in buildings)
        {
            if (building != start)
            {
                List<string> path = DijkstraAlgo(start, building, buildings, distances);

                // check if any duplicate path (Example shortestPaths has A->D->B and path is A->D)
                if (shortestPaths.Count > 0 && shortestPaths.Any(x => x.Take(2).SequenceEqual(path.Take(2))))
                {
                    // get duplicate
                    List<string> duplicate = shortestPaths.Where(x => x.Take(2).SequenceEqual(path.Take(2))).First();

                    // condition to check if path is longer than existing, if true then remove it, else just continue the code
                    if (duplicate.Count < path.Count)
                    {
                        shortestPaths.Remove(duplicate);
                    }
                    else
                    {
                        continue;
                    }
                }

                shortestPaths.Add(path);
            }
        }

        return shortestPaths;
    }

    public static int GetLengths(List<string> path, Dictionary<(string, string), int> distances)
    {
        int length = 0;
        for (int i = 0; i < path.Count - 1; i++)
        {
            (string, string) key = (path[i], path[i + 1]);
            if (distances.TryGetValue(key, out int value))
            {
                length += value;
            }
        }
        return length;
    }

    private static List<string> DijkstraAlgo(string start, string end, List<string> buildings, Dictionary<(string, string), int> distances)
    {
        Dictionary<string, int> distance = [];
        Dictionary<string, string> previous = [];
        HashSet<string> unvisited = new(buildings);

        foreach (string building in buildings)
        {
            distance[building] = int.MaxValue;
            previous[building] = null;
        }
        distance[start] = 0;

        while (unvisited.Count > 0)
        {
            string current = GetClosestCity(distance, unvisited);
            unvisited.Remove(current);

            if (current == end)
                break;

            foreach (string neighbor in buildings)
            {
                (string, string) key = (current, neighbor);
                if (distances.TryGetValue(key, out int value) && unvisited.Contains(neighbor))
                {
                    int alt = distance[current] + value;
                    if (alt < distance[neighbor])
                    {
                        distance[neighbor] = alt;
                        previous[neighbor] = current;
                    }
                }
            }
        }

        List<string> path = [];
        for (string at = end; at != null; at = previous[at])
        {
            path.Add(at);
        }
        path.Reverse();

        return path;
    }

    private static string GetClosestCity(Dictionary<string, int> dist, HashSet<string> unvisited)
    {
        int minDistance = int.MaxValue;
        string closestCity = null;

        foreach (string building in unvisited)
        {
            if (dist[building] < minDistance)
            {
                minDistance = dist[building];
                closestCity = building;
            }
        }

        return closestCity;
    }
}