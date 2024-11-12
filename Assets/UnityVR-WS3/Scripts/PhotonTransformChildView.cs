
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Photon.Pun.PhotonView))]
public class PhotonTransformChildView : MonoBehaviourPunCallbacks, IPunObservable
{
    public bool SynchronizePosition = true;
    public bool SynchronizeRotation = true;
    public bool SynchronizeScale = false;
    public List<Transform> SynchronizedChildTransform;
    private List<Vector3> localPositionList = new List<Vector3>();
    private List<Quaternion> localRotationList = new List<Quaternion>();
    private List<Vector3> localScaleList = new List<Vector3>();



    // Start is called before the first frame update
    void Awake()
    {
        SynchronizedChildTransform.ForEach(child => {
            localPositionList.Add(child.position);
            localRotationList.Add(child.rotation);
            localScaleList.Add(child.localScale);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine){
            for(int index = 0; index < SynchronizedChildTransform.Count;index++){
                if (SynchronizePosition) {
                    localPositionList[index] = SynchronizedChildTransform[index].position;
                }
                if (SynchronizeRotation) {
                    localRotationList[index] = SynchronizedChildTransform[index].rotation;
                }
                if (SynchronizeScale) {
                    localScaleList[index] = SynchronizedChildTransform[index].localScale;
                }
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            for(int index = 0; index < SynchronizedChildTransform.Count;index++){

                if (SynchronizePosition) {
                    stream.SendNext(SynchronizedChildTransform[index].position);
                }
                if (SynchronizeRotation) {
                    stream.SendNext(SynchronizedChildTransform[index].rotation);
                }
                if (SynchronizeScale) {
                    stream.SendNext(SynchronizedChildTransform[index].localScale);
                }

            }
        }else{ 
            for(int index = 0; index < SynchronizedChildTransform.Count;index++){

                if (SynchronizePosition)
                    SynchronizedChildTransform[index].position = (Vector3)stream.ReceiveNext();
                if (SynchronizeRotation)
                    SynchronizedChildTransform[index].rotation = (Quaternion)stream.ReceiveNext();
                if (SynchronizeScale)
                    SynchronizedChildTransform[index].localScale = (Vector3)stream.ReceiveNext();
            }
        }
    }
}
