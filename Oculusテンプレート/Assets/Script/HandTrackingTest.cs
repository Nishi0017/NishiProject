using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandTrackingTest : MonoBehaviour
{
    public TextMeshProUGUI ThumbTest;
    public TextMeshProUGUI IndexTest;
    public TextMeshProUGUI MiddleTest;
    public TextMeshProUGUI RingTest;
    public TextMeshProUGUI PinkyTest;
    
    bool ThumbChange = true;
    bool IndexChange = true;
    bool MiddleChange = true;
    bool RingChange = true;
    bool PinkyChange = true;
    

    [SerializeField]
    private OVRSkeleton _skeleton; //右手、もしくは左手の Bone情報

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
         for (var index = 0; index < boneids.Length-1; index++)
         {
             var v = (_skeleton.Bones[(int)boneids[index+1]].Transform.position - _skeleton.Bones[(int)boneids[index]].Transform.position).normalized;
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
        var isThumbStraight = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Thumb0, OVRSkeleton.BoneId.Hand_Thumb1, OVRSkeleton.BoneId.Hand_Thumb2, OVRSkeleton.BoneId.Hand_Thumb3, OVRSkeleton.BoneId.Hand_ThumbTip);
        var isIndexStraight = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Index1, OVRSkeleton.BoneId.Hand_Index2, OVRSkeleton.BoneId.Hand_Index3, OVRSkeleton.BoneId.Hand_IndexTip);
        var isMiddleStraight = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Middle1, OVRSkeleton.BoneId.Hand_Middle2, OVRSkeleton.BoneId.Hand_Middle3, OVRSkeleton.BoneId.Hand_MiddleTip);
        var isRingStraight = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Ring1, OVRSkeleton.BoneId.Hand_Ring2, OVRSkeleton.BoneId.Hand_Ring3, OVRSkeleton.BoneId.Hand_RingTip);
        var isPinkyStraight = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Pinky0, OVRSkeleton.BoneId.Hand_Pinky1, OVRSkeleton.BoneId.Hand_Pinky2, OVRSkeleton.BoneId.Hand_Pinky3, OVRSkeleton.BoneId.Hand_PinkyTip);




        if(isThumbStraight && ThumbChange)
        {
            ThumbTest.text = "Thumb:ture";
            ThumbChange = false;
        }
        else if(!isThumbStraight && !ThumbChange)
        {
            ThumbTest.text = "Thumb:false";
            ThumbChange = true;
        }

        if(isIndexStraight && IndexChange)
        {
            IndexTest.text = "Index:ture";
            IndexChange = false;
        }
        else if(!isIndexStraight && !IndexChange)
        {
            IndexTest.text = "Index:false";
            IndexChange = true;
        }

        if(isMiddleStraight && MiddleChange)
        {
            MiddleTest.text = "Middle:ture";
            MiddleChange = false;
        }
        else if(!isMiddleStraight && !MiddleChange)
        {
            MiddleTest.text = "Middle:false";
            MiddleChange = true;
        }

        if(isRingStraight && RingChange)
        {
            RingTest.text = "Ring:ture";
            RingChange = false;
        }
        else if(!isRingStraight && !RingChange)
        {
            RingTest.text = "Ring:false";
            RingChange = true;
        }

        if(isPinkyStraight && PinkyChange)
        {
            PinkyTest.text = "Pinky:ture";
            PinkyChange = false;
        }
        else if(!isPinkyStraight && !PinkyChange)
        {
            PinkyTest.text = "Pinky:false";
            PinkyChange = true;
        }
    }
}
