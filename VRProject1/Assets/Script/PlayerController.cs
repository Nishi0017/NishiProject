using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static OVRPlugin;

public class PlayerController : MonoBehaviour
{
    public float _threshold = 0.27f;
    [Header("指パッチン")]public GameObject prefab_FingerSnap;

    [SerializeField]
    private OVRSkeleton _oVRSkeleton; //右手、もしくは左手の Bone情報
    [SerializeField]
    private OVRHand _oVRHand;

    private bool _isThumbStraight_old;
    private bool _isInsexStraight_old;
    private bool _isMiddleStraight_old;
    private bool _isRingStraight_old;
    private bool _isPinkyStraight_old;

    public Vector3 indexDirection;

    private void Start()
    {
        // ハンドトラッキングをしていない、またはハンドトラッキングの信用度が低ければ誤作動を防ぐために無効にする
        if (!_oVRHand.IsTracked || _oVRHand.HandConfidence.Equals(OVRHand.TrackingConfidence.Low)) return;

        var isThumbStraight_old = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Thumb0, OVRSkeleton.BoneId.Hand_Thumb1, OVRSkeleton.BoneId.Hand_Thumb2, OVRSkeleton.BoneId.Hand_Thumb3, OVRSkeleton.BoneId.Hand_ThumbTip);
        var isIndexStraight_old = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Index1, OVRSkeleton.BoneId.Hand_Index2, OVRSkeleton.BoneId.Hand_Index3, OVRSkeleton.BoneId.Hand_IndexTip);
        var isMiddleStraight_old = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Middle1, OVRSkeleton.BoneId.Hand_Middle2, OVRSkeleton.BoneId.Hand_Middle3, OVRSkeleton.BoneId.Hand_MiddleTip);
        var isRingStraight_old = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Ring1, OVRSkeleton.BoneId.Hand_Ring2, OVRSkeleton.BoneId.Hand_Ring3, OVRSkeleton.BoneId.Hand_RingTip);
        var isPinkyStraight_old = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Pinky0, OVRSkeleton.BoneId.Hand_Pinky1, OVRSkeleton.BoneId.Hand_Pinky2, OVRSkeleton.BoneId.Hand_Pinky3, OVRSkeleton.BoneId.Hand_PinkyTip);

    }

    /// <summary>
    /// 指定した全てのBoneIDが直線状にあるかどうか調べる
    /// </summary>
    /// <param name="threshold">閾値 1に近いほど厳しい</param>
    /// <param name="boneids"></param>
    /// <returns></returns>
    private bool IsStraight(float threshold, params OVRSkeleton.BoneId[] boneids)
    {
        if (boneids.Length < 3) return false;   //調べようがない
        Vector3? oldVec = null;
        var dot = 1.0f;
        for (var index = 0; index < boneids.Length - 1; index++)
        {
            var v = (_oVRSkeleton.Bones[(int)boneids[index + 1]].Transform.position - _oVRSkeleton.Bones[(int)boneids[index]].Transform.position).normalized;
            if (oldVec.HasValue)
            {
                dot *= Vector3.Dot(v, oldVec.Value); //内積の値を総乗していく
            }
            oldVec = v;//ひとつ前の指ベクトル
        }
        return dot >= threshold; //指定したBoneIDの内積の総乗が閾値を超えていたら直線とみなす
    
    }

    private void Update()
    {
        // ハンドトラッキングをしていない、またはハンドトラッキングの信用度が低ければ誤作動を防ぐために無効にする
        if (!_oVRHand.IsTracked || _oVRHand.HandConfidence.Equals(OVRHand.TrackingConfidence.Low)) return;

        var isThumbStraight = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Thumb0, OVRSkeleton.BoneId.Hand_Thumb1, OVRSkeleton.BoneId.Hand_Thumb2, OVRSkeleton.BoneId.Hand_Thumb3, OVRSkeleton.BoneId.Hand_ThumbTip);
        var isIndexStraight = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Index1, OVRSkeleton.BoneId.Hand_Index2, OVRSkeleton.BoneId.Hand_Index3, OVRSkeleton.BoneId.Hand_IndexTip);
        var isMiddleStraight = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Middle1, OVRSkeleton.BoneId.Hand_Middle2, OVRSkeleton.BoneId.Hand_Middle3, OVRSkeleton.BoneId.Hand_MiddleTip);
        var isRingStraight = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Ring1, OVRSkeleton.BoneId.Hand_Ring2, OVRSkeleton.BoneId.Hand_Ring3, OVRSkeleton.BoneId.Hand_RingTip);
        var isPinkyStraight = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Pinky0, OVRSkeleton.BoneId.Hand_Pinky1, OVRSkeleton.BoneId.Hand_Pinky2, OVRSkeleton.BoneId.Hand_Pinky3, OVRSkeleton.BoneId.Hand_PinkyTip);

        var indexTipPos = _oVRSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position;
        var index1Pos = _oVRSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_Index1].Transform.position;
        
        float indexDirection_x = indexTipPos.x - index1Pos.x;
        float indexDirection_y = indexTipPos.y - index1Pos.y;
        float indexDirection_z = indexTipPos.z - index1Pos.z;

        indexDirection = new Vector3(indexDirection_x, indexDirection_y, indexDirection_z);
 

        //指パッチン
        if (_isMiddleStraight_old && !isMiddleStraight && isIndexStraight)
        {
            Instantiate(
                prefab_FingerSnap, //クローンするプレハブ
                position: _oVRSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position, //生成場所：人差し指の先
                rotation: Quaternion.identity //回転なし
            );
        }


        if(_isThumbStraight_old && !isThumbStraight && isIndexStraight && !isMiddleStraight && !isRingStraight && !isPinkyStraight)
        {
            Instantiate(
                prefab_FingerSnap, //クローンするプレハブ
                position: _oVRSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position, //生成場所：人差し指の先
                rotation: Quaternion.identity //回転なし
            );
        }


        _isThumbStraight_old = isThumbStraight;
        _isInsexStraight_old = isIndexStraight;
        _isMiddleStraight_old = isMiddleStraight;
        _isRingStraight_old = isRingStraight;
        _isPinkyStraight_old = isPinkyStraight;
}
}
