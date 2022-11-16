using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicScript : MonoBehaviour
{

    public GameObject[] _magicCirclesOnHands;
    public GameObject _magicCircle_onGround;
    public GameObject _magicCircle_onGround_generateTrans;

    private void OnTriggerEnter(Collider other)
    {
        // _anotherHandTag��"RightHand"
        // �R���C�_�[���Փ˂������肪�E��Ȃ�
        if (other.gameObject.tag == "RightHand")
        {
            Debug.Log("AA");
            //����̍b�̖��@�w��L��������
            foreach (var magicCircleOnHands in _magicCirclesOnHands)
            {
                magicCircleOnHands.SetActive(true);
            }
        }
    // �R���C�_�[���Փ˂������肪���Ȃ�
        else if(other.gameObject.tag == "Floor")
        {
            // ����̘B���w���A�N�e�B�u�Ȃ�
            if(!_magicCirclesOnHands[0].activeSelf) return;

            // �n�ʂɖ��@�w���o��
            // _magicCircle_onGround_generateTrans�̓v���C���[�O��1m�ɔz�u����Transform
            Instantiate(
                _magicCircle_onGround,
                _magicCircle_onGround_generateTrans.transform.position,
                _magicCircle_onGround_generateTrans.transform.rotation
            );

            // �n�ʘB���ɂ͘B���G�l���M�[�H���g���̂ŗ���̘B���w���A�N�e�B�u�ɂ���
            foreach (var magicCircleOnHands in _magicCirclesOnHands)
            {
                magicCircleOnHands.SetActive(false);
            }
        }
    }
}
