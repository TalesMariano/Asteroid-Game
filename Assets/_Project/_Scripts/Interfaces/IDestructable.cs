using System;

public interface IDestructable
{
    Action OnDestroied { get; set; }

    void Destroy();
}