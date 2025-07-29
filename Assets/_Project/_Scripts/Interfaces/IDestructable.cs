using System;

public interface IDestructable
{
    Action OnDestroyed { get; set; }

    void Destroy();
}