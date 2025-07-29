using System.Collections.Generic;

public interface ITracker<T>
{
    bool ObjectDetected { get; }
    List<T> ObjectList { get; }
}

