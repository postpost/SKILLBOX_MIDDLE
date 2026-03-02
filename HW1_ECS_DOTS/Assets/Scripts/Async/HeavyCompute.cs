using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Jobs;

public class HeavyCompute : MonoBehaviour
{
    #region async/await Task
    //public async void HeavyMethod()
    // {
    //     //Thread.Sleep(2000);
    //     await Task.Run(SuperHeavyMethod);
    //     Debug.Log("<color=green> Heavy Finish </color>");
    // }

    // async Task SuperHeavyMethod()
    // {
    //     await Task.Delay(5000); // почему здесь нужен await
    //     Thread.Sleep(3000);     // а здесь await не нужен?
    // }

    #endregion

    #region Jobs/NativeArrays

    //NativeArray<float> result;
    //private HeavyJob jobData;

    //public void HeavyMethod()
    //{
    //    //делаем из структуры ссылочный тип
    //    result = new NativeArray<float>(1, Allocator.TempJob); //TempJob: недолго, неуправляемая память

    //    jobData = new HeavyJob
    //    {
    //        _result = result,
    //    };

    //    JobHandle handle = jobData.Schedule();
    //    handle.Complete();
    //    Debug.Log(result[0]);
    //    result.Dispose();
    //}

    #endregion

    #region JobParallel

    NativeArray<float> result;
    NativeArray<float> input;
    private HeavyJob jobData;
    private JobHandle handle;
    private bool isProcessing = false;

    public void HeavyMethod()
    {
        //делаем из структуры ссылочный тип
        result = new NativeArray<float>(100, Allocator.Persistent);  //Persistent: тк вычисления займут больше нескольких кадров
        input = new NativeArray<float> (100, Allocator.Persistent);
        jobData = new HeavyJob
        {
            _result = result,
            _number = input
        };

        handle = jobData.Schedule(result.Length, 33); //3 в один пакет, innerloopBatchCount
        isProcessing = true;
        //handle.Complete(); //фризит гл поток
    }

    private void Update()
    {
        if (isProcessing && handle.IsCompleted)
        {
            handle.Complete();
            Debug.Log($"result: {result.Length}; input: {input.Length}");
            result.Dispose();
            input.Dispose();
            isProcessing = false;
        }
    }

    #endregion
}

#region IJob
//public struct HeavyJob : IJob
//{
//    public NativeArray<float> _result;
//    public void Execute()
//    {
//        _result[0] = 10;
//    }
//}
#endregion

#region IJobParallelFor
public struct HeavyJob : IJobParallelFor
{
    public NativeArray<float> _number;
    public NativeArray<float> _result;
    public void Execute(int index)
    {
        _result[index] = Mathf.Sqrt(_number[index]);
    }
}
#endregion

#region IJobParallelForTransform
public struct HeavyJobTransform : IJobParallelForTransform
{
    public NativeArray<float> _number;
    public NativeArray<float> _result;
    public void Execute(int index)
    {
        _result[index] = Mathf.Sqrt(_number[index]);
    }

    public void Execute(int index, TransformAccess transform)
    {
        throw new System.NotImplementedException();
    }
}
#endregion
