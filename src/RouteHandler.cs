using RouteInterface;

namespace Routes;

public class RouteHandler(WebApplication app)
{
    private readonly LinkedList<IRoute> _routes = new();

    public RouteHandler Add(IRoute route)
    {
        _ = _routes.AddLast(route);
        return this;
    }

    public void Remove(IRoute route)
    {
        _routes.Remove(route);
    }

    public void Map()
    {
        foreach (var route in _routes)
        {
            route.Map(app);
        }
    }
}