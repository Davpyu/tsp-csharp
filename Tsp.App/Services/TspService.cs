namespace Tsp.App.Services;

public class TspService(
    string start,
    List<string> _buildings,
    Dictionary<(string, string), int> _distances
) {
    private readonly string _start = start;
    private readonly List<string> _buildings = _buildings;
    private readonly Dictionary<(string, string), int> _distances = _distances;

    public List<(List<string>, int)> GetPaths()
    {
        List<(List<string>, int)> shortestPaths = [];

        foreach (string building in _buildings)
        {
            if (building != _start)
            {
                (List<string> path, int length) = FindShortestPathDijkstra(building);

                // check if any duplicate path (Example shortestPaths has A->D->B and path is A->D)
                if (shortestPaths.Count > 0 && shortestPaths.Any(x => x.Item1.Take(2).SequenceEqual(path.Take(2))))
                {
                    // get duplicate
                    (List<string>, int) duplicate = shortestPaths.Where(x => x.Item1.Take(2).SequenceEqual(path.Take(2))).First();

                    // condition to check if path is longer than existing, if true then remove it, else just continue the code
                    if (duplicate.Item1.Count < path.Count)
                    {
                        shortestPaths.Remove(duplicate);
                    }

                    else
                    {
                        continue;
                    }
                }

                shortestPaths.Add((path, length));
            }
        }

        return shortestPaths;
    }

    private int GetLengths(List<string> path)
    {
        int length = 0;

        for (int i = 0; i < path.Count - 1; i++)
        {
            (string, string) key = (path[i], path[i + 1]);
            if (_distances.TryGetValue(key, out int value))
            {
                length += value;
            }
        }

        return length;
    }

    private (List<string>, int) FindShortestPathDijkstra(string end)
    {
        (Dictionary<string, int> distance, Dictionary<string, string> previous, HashSet<string> unvisited) = InitializeVariable();

        CheckPathLengthPerNeighbor(end, distance, previous, unvisited);

        return ProcessPath(end, previous);
    }

    private static string GetClosestBuilding(Dictionary<string, int> dist, HashSet<string> unvisited)
    {
        // use max value to make it easier to compare rather than using 0 since we need to check for shorter path rather than longer path
        int minDistance = int.MaxValue;
        string closestBuilding = null;

        foreach (string building in unvisited)
        {
            int currentDistance = dist.First(x => x.Key == building).Value;
            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                closestBuilding = building;
            }
        }

        return closestBuilding;
    }

    private (Dictionary<string, int>, Dictionary<string, string>, HashSet<string>) InitializeVariable()
    {
        Dictionary<string, int> distance = [];
        Dictionary<string, string> previous = [];

        // make variable to list all unvisited building
        HashSet<string> unvisited = new(_buildings);

        foreach (string building in _buildings)
        {
            distance.Add(building, int.MaxValue);
            previous.Add(building, null);
        }

        distance[_start] = 0;

        return (distance, previous, unvisited);
    }

    private void CheckPathLengthPerNeighbor(string end, Dictionary<string, int> distance, Dictionary<string, string> previous, HashSet<string> unvisited)
    {
        while (unvisited.Count > 0)
        {
            string current = GetClosestBuilding(distance, unvisited);

            // remove it since it has been "visited" currently
            unvisited.Remove(current);

            // since we dont need to loop on the same city, just break the loop and goes to other logic
            if (current == end)
            {
                break;
            }

            foreach (string neighbor in _buildings)
            {
                // get key for variable distances to check if there is any relation from current building to current neighbor
                (string, string) key = (current, neighbor);

                // try to get distance from previous key and check if neighbor is unvisited or not
                if (_distances.TryGetValue(key, out int value) && unvisited.Contains(neighbor))
                {
                    // if found, add the distance with value from the condition before as new variable
                    int alt = distance.First(x => x.Key == current).Value + value;
                    int distanceNeighbor = distance.First(x => x.Key == neighbor).Value;

                    // if new variable is shorter than current distance from building to neighbor than replace it
                    if (alt < distanceNeighbor)
                    {
                        distance[neighbor] = alt;
                        previous[neighbor] = current;
                    }
                }
            }
        }
    }

    private (List<string>, int) ProcessPath(string end, Dictionary<string, string> previous)
    {
        List<string> path = [];

        for (string at = end; at != null; at = previous[at])
        {
            path.Add(at);
        }

        // reverse it since it was started from "end"
        path.Reverse();

        return (path, GetLengths(path));
    }
}