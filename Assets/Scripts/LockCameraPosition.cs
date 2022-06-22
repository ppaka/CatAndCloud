using UnityEngine;
using Cinemachine;
 
/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[ExecuteInEditMode] [SaveDuringPlay] [AddComponentMenu("")] // Hide in menu
public class LockCameraPosition : CinemachineExtension
{
    public bool isLockXPos,isLockYPos,isLockZPos;
    public float xPos, yPos, zPos;
    public Transform targetGroup;
 
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            if(isLockXPos)
            {
                pos.x = xPos;
                targetGroup.transform.position = new Vector3(xPos, targetGroup.transform.position.y, targetGroup.transform.position.z);
            }
            if(isLockYPos)
            {
                pos.y = yPos;
                targetGroup.transform.position = new Vector3(targetGroup.transform.position.x, yPos, targetGroup.transform.position.z);
            }
            if(isLockZPos)
            {
                pos.z = zPos;
                targetGroup.transform.position = new Vector3(targetGroup.transform.position.x, targetGroup.transform.position.y, zPos);
            }

            state.RawPosition = pos;
        }
    }
}