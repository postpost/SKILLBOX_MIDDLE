using UnityEngine;
using UnityEngine.VFX;

public class GlobalVFX : MonoBehaviour
{
    public static GlobalVFX Instance;
    public VisualEffect vfx;

    private void Awake()
    {
        Instance = this;
    }

    public void SendData(Vector3 position)
    {
        //send data to vfx graph blackboard
        vfx.SetVector3("BulletPos", position); 
        
        //invoke method in vfx graph OnUpdate
        vfx.SendEvent("OnUpdate");
    }
}
