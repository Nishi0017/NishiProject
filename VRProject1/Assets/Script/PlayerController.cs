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
    private OVRSkeleton _oVRSkeleton; //�E��A�������͍���� Bone���
    [SerializeField]
    private OVRHand _oVRHand;
    private bool _isMiddleStraight_old;

    private void Start()
    {
        _isMiddleStraight_old = IsStraight(_threshold, OVRSkeleton.BoneId.Hand_Middle1, OVRSkeleton.BoneId.Hand_Middle2, OVRSkeleton.BoneId.Hand_Middle3, OVRSkeleton.BoneId.Hand_MiddleTip);
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

        // ���w�Ɛl�����w�̋Ȃ��󋵂��擾����
        // �Ȃ�臒l(_threshold)��0.27�����x�ǂ�����
        var isMiddleStraight = IsStraight(_threshold, OVRSkeleton.BoneId.Hand_Middle1, OVRSkeleton.BoneId.Hand_Middle2, OVRSkeleton.BoneId.Hand_Middle3, OVRSkeleton.BoneId.Hand_MiddleTip);
        var isIndexStraight = IsStraight(_threshold, OVRSkeleton.BoneId.Hand_Index1, OVRSkeleton.BoneId.Hand_Index2, OVRSkeleton.BoneId.Hand_Index3, OVRSkeleton.BoneId.Hand_IndexTip);
        // �O��̃t���[���Œ��w���L�тĂ��č���̃t���[���Œ��w���Ȃ����Ă���Β��w���Ȃ����u�Ԃ����
        // ���̂Ƃ��ɐl�����w���L�тĂ���Ύw�p�b�`�����������
        if (!isMiddleStraight && _isMiddleStraight_old && isIndexStraight)
        {
            // �v���n�u�����Ă��������̃G�t�F�N�g�𐶐�����
            // �����ꏊ�͐l�����w�̐�ɂ���Ƃ�����ۂ�
            Instantiate(
                _fire_base,
                position: _oVRSkeleton.Bones[(int)OVRSkeleton.BoneId.Hand_IndexTip].Transform.position,
                rotation: Quaternion.identity
            );
        }

        _isMiddleStraight_old = isMiddleStraight;
    }
}
