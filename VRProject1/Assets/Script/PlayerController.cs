using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static OVRPlugin;

public class PlayerController : MonoBehaviour
{
    public float _threshold = 0.27f;
    [Header("�w�p�b�`��")]public GameObject prefab_FingerSnap;

    [SerializeField]
    private OVRSkeleton _oVRSkeleton; //�E��A�������͍���� Bone���
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
        // �n���h�g���b�L���O�����Ă��Ȃ��A�܂��̓n���h�g���b�L���O�̐M�p�x���Ⴏ��Ό�쓮��h�����߂ɖ����ɂ���
        if (!_oVRHand.IsTracked || _oVRHand.HandConfidence.Equals(OVRHand.TrackingConfidence.Low)) return;

        var isThumbStraight_old = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Thumb0, OVRSkeleton.BoneId.Hand_Thumb1, OVRSkeleton.BoneId.Hand_Thumb2, OVRSkeleton.BoneId.Hand_Thumb3, OVRSkeleton.BoneId.Hand_ThumbTip);
        var isIndexStraight_old = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Index1, OVRSkeleton.BoneId.Hand_Index2, OVRSkeleton.BoneId.Hand_Index3, OVRSkeleton.BoneId.Hand_IndexTip);
        var isMiddleStraight_old = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Middle1, OVRSkeleton.BoneId.Hand_Middle2, OVRSkeleton.BoneId.Hand_Middle3, OVRSkeleton.BoneId.Hand_MiddleTip);
        var isRingStraight_old = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Ring1, OVRSkeleton.BoneId.Hand_Ring2, OVRSkeleton.BoneId.Hand_Ring3, OVRSkeleton.BoneId.Hand_RingTip);
        var isPinkyStraight_old = IsStraight(0.8f, OVRSkeleton.BoneId.Hand_Pinky0, OVRSkeleton.BoneId.Hand_Pinky1, OVRSkeleton.BoneId.Hand_Pinky2, OVRSkeleton.BoneId.Hand_Pinky3, OVRSkeleton.BoneId.Hand_PinkyTip);

    }

    /// <summary>
    /// �w�肵���S�Ă�BoneID��������ɂ��邩�ǂ������ׂ�
    /// </summary>
    /// <param name="threshold">臒l 1�ɋ߂��قǌ�����</param>
    /// <param name="boneids"></param>
    /// <returns></returns>
    private bool IsStraight(float threshold, params OVRSkeleton.BoneId[] boneids)
    {
        if (boneids.Length < 3) return false;   //���ׂ悤���Ȃ�
        Vector3? oldVec = null;
        var dot = 1.0f;
        for (var index = 0; index < boneids.Length - 1; index++)
        {
            var v = (_oVRSkeleton.Bones[(int)boneids[index + 1]].Transform.position - _oVRSkeleton.Bones[(int)boneids[index]].Transform.position).normalized;
            if (oldVec.HasValue)
            {
                dot *= Vector3.Dot(v, oldVec.Value); //���ς̒l�𑍏悵�Ă���
            }
            oldVec = v;//�ЂƂO�̎w�x�N�g��
        }
        return dot >= threshold; //�w�肵��BoneID�̓��ς̑��悪臒l�𒴂��Ă����璼���Ƃ݂Ȃ�
    
    }

    private void Update()
    {
        // �n���h�g���b�L���O�����Ă��Ȃ��A�܂��̓n���h�g���b�L���O�̐M�p�x���Ⴏ��Ό�쓮��h�����߂ɖ����ɂ���
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
 

        //�w�p�b�`��
        if (_isMiddleStraight_old && !isMiddleStraight && isIndexStraight)
        {
            Instantiate(
                prefab_FingerSnap, //�N���[������v���n�u
                position: _oVRSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position, //�����ꏊ�F�l�����w�̐�
                rotation: Quaternion.identity //��]�Ȃ�
            );
        }


        if(_isThumbStraight_old && !isThumbStraight && isIndexStraight && !isMiddleStraight && !isRingStraight && !isPinkyStraight)
        {
            Instantiate(
                prefab_FingerSnap, //�N���[������v���n�u
                position: _oVRSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position, //�����ꏊ�F�l�����w�̐�
                rotation: Quaternion.identity //��]�Ȃ�
            );
        }


        _isThumbStraight_old = isThumbStraight;
        _isInsexStraight_old = isIndexStraight;
        _isMiddleStraight_old = isMiddleStraight;
        _isRingStraight_old = isRingStraight;
        _isPinkyStraight_old = isPinkyStraight;
}
}
