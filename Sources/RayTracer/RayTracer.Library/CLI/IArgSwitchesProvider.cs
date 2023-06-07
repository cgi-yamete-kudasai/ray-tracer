using System.Collections.Generic;

namespace RayTracer.Library.CLI;

public interface IArgSwitchesProvider
{
    IReadOnlyList<object> Listeners { get; }
}
