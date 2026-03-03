
public interface ICapable
{
    void Tick(float deltaTime);
    void Activate(float capabilityAmount);
    void Reset();
    bool isFinished { get; }

    float ElapsedActiveTime { get; }
    float TotalActivationTime { get; }
}
