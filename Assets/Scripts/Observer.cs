using UnityEngine;

public interface ISubject
{
    void ResisterObserver(IObserver observer);

    void RemoveObserver(IObserver observer);


    void NotifyObservers();
}

public interface IObserver
{
    void UpdateData();
}
