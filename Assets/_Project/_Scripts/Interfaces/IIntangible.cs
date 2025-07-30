using System;

public interface IIntangible 
{
    Action<bool> OnChangeIntangible { get; set; }
}
