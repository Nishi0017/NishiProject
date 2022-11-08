using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float _threshold = 0.27f;
    public GameObject _fire_base;

    [SerializeField]
    private OVRSkeleton _oVRSkeleton; //右手、もしくは左手の Bone情報
    [SerializeField]
    private OVRHand _oVRHand;
    private bool _isMiddleStraight_old;

    private void Start()
    {
        _isMiddleStraight_old = IsStraight(_threshold, OVRSkeleton.BoneId.Hand_Middle1, OVRSkeleton.BoneId.Hand_Middle2, OVRSkeleton.BoneId.Hand_Middle3, OVRSkeleton.BoneId.Hand_MiddleTip);
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

        // 中指と人差し指の曲げ状況を取得する
        // 曲げ閾値(_threshold)は0.27が丁度良かった
        var isMiddleStraight = IsStraight(_threshold, OVRSkeleton.BoneId.Hand_Middle1, OVRSkeleton.BoneId.Hand_Middle2, OVRSkeleton.BoneId.Hand_Middle3, OVRSkeleton.BoneId.Hand_MiddleTip);
        var isIndexStraight = IsStraight(_threshold, OVRSkeleton.BoneId.Hand_Index1, OVRSkeleton.BoneId.Hand_Index2, OVRSkeleton.BoneId.Hand_Index3, OVRSkeleton.BoneId.Hand_IndexTip);
        // 前回のフレームで中指が伸びていて今回のフレームで中指が曲がっていれば中指を曲げた瞬間だよね
        // そのときに人差し指が伸びていれば指パッチンをしたよね
        if (!isMiddleStraight && _isMiddleStraight_old && isIndexStraight)
        {
            // プレハブ化しておいた炎のエフェクトを生成する
            // 生成場所は人差し指の先にするとそれっぽい
            Instantiate(
                _fire_base,
                position: _oVRSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position,
                rotation: Quaternion.identity
            );
        }

        _isMiddleStraight_old = isMiddleStraight;
    }
}
