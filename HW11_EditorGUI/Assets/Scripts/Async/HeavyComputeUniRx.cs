using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeavyComputeUniRx : MonoBehaviour
{
   public void HeavyMethod()
    {
        var clickStream = Observable.EveryUpdate().
            Where(_ => Mouse.current.leftButton.wasPressedThisFrame);

        clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
            .Where(xs => xs.Count >= 2)
            .Subscribe(xs => Debug.Log($"Double Click detected. Count: {xs.Count}"));
    }

    public void NetworkHeavyMethod()
    {
        var parallel = Observable.WhenAll(
            ObservableWWW.Get("http://google.com"),
            ObservableWWW.Get("http://yandex.ru"),
            ObservableWWW.Get("http://unity3d.com")
            );
        parallel.Subscribe(xs =>
        {
            Debug.Log(xs[0].Substring(0, 100));
            Debug.Log(xs[1].Substring(0, 100));
            Debug.Log(xs[2].Substring(0, 100));

        });
    }

    public void NetworkHeavyMethodParallel()
    {
        var heavyMethod = Observable.Start(() =>
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
            Debug.Log("Finish_1");
            return "HeavyMethod_1";
        }
        );

        var heavyMethod_2 = Observable.Start(() =>
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));
            Debug.Log("Finish_2");
            return "HeavyMethod_2";
        }
        );

        Observable.WhenAll(heavyMethod, heavyMethod_2).ObserveOnMainThread()
            .Subscribe(xs=>
            {
                Debug.Log($"{xs[0]}");
                Debug.Log($"{xs[1]}");
            });
    }

   // private Func<int, bool> isPositive = x => x > 0;
}
