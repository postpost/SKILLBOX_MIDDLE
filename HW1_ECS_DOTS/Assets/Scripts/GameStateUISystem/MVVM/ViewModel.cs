using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class ViewModel : MonoBehaviour, INotifyPropertyChanged
{
    private int _health;

    [Binding]
    public int Health
    {
        get => _health;
        set
        {
            if(_health.Equals(value)) return;
            _health = value;
            OnPropertyChanged(nameof(Health));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
