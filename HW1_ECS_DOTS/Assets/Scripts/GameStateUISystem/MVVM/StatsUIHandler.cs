using UnityEngine;

public class StatsUIHandler : MonoBehaviour
{
    public static StatsUIHandler Instance;
    public ViewModel ViewModel => _viewModel;
    
    private ViewModel _viewModel;


    private void Awake()
    {
        Instance = this;
        if (_viewModel == null) _viewModel = GetComponent<ViewModel>();
    }
}
