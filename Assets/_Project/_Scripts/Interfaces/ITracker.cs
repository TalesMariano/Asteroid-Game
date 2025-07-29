using System.Collections.Generic;

public interface ITracker<T>
{
    List<T> listItems { get; }
}

