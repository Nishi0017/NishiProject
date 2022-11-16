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
        // _anotherHandTagは"RightHand"
        // コライダーが衝突した相手が右手なら
        if (other.gameObject.tag == "RightHand")
        {
            Debug.Log("AA");
            //両手の甲の魔法陣を有効化する
            foreach (var magicCircleOnHands in _magicCirclesOnHands)
            {
                magicCircleOnHands.SetActive(true);
            }
        }
    // コライダーが衝突した相手が床なら
        else if(other.gameObject.tag == "Floor")
        {
            // 左手の錬成陣がアクティブなら
            if(!_magicCirclesOnHands[0].activeSelf) return;

            // 地面に魔法陣を出す
            // _magicCircle_onGround_generateTransはプレイヤー前方1mに配置したTransform
            Instantiate(
                _magicCircle_onGround,
                _magicCircle_onGround_generateTrans.transform.position,
                _magicCircle_onGround_generateTrans.transform.rotation
            );

            // 地面錬成には錬成エネルギー？を使うので両手の錬成陣を非アクティブにする
            foreach (var magicCircleOnHands in _magicCirclesOnHands)
            {
                magicCircleOnHands.SetActive(false);
            }
        }
    }
}
