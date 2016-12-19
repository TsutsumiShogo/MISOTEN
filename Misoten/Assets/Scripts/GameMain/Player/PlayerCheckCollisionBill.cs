using UnityEngine;
using System.Collections;

public class PlayerCheckCollisionBill : MonoBehaviour {

    private PlayerUnit playerUnit;
    public GM_MathFlowerParam billParam = null;

	// Use this for initialization
	void Awake () {
        playerUnit = transform.parent.GetComponent<PlayerUnit>();
        Init();
	}
    public void Init(){
        billParam = null;
    }

    //ビルが目の前にあればtrueを返す
    public bool CheckCollisionBill()
    {
        if (billParam == null)
        {
            return false;
        }

        //z比較してプレイヤーが手前側ならビル存在とみなす
        if (billParam.transform.position.z < playerUnit.transform.position.z)
        {
            return false;
        }

        return true;
    }

    //ビルの方向へ向く回転情報
    public Quaternion GetBillRot()
    {
        //プレイヤーの現在の回転情報を取得
        Quaternion rot = transform.parent.rotation;

        //ビルが無ければ何もせず返す
        if (billParam == null)
        {
            return rot;
        }
        
        //ビル方向へのベクトル算出
        Vector3 _billVec;
        _billVec = billParam.transform.position - transform.parent.position;

        //0ベクトル回避
        if (_billVec.x != 0 || _billVec.z != 0)
        {
            //回転を計算
            float angle;
            angle = Mathf.Atan2(_billVec.x, _billVec.z);
            rot = Quaternion.AngleAxis(angle / 3.1415f * 180.0f, Vector3.up);
        }

        //回転情報を返す
        return rot;
    }

    //ビル専用の成長関数
    public void GrowthBill()
    {
        //成長対象が存在するかチェック
        if (billParam == null)
        {
            return;
        }
        //中ビルでも大ビルでも無ければ何もしない
        if (billParam.flowerType != GM_MathFlowerParam.EFlowerType.Bill && billParam.flowerType != GM_MathFlowerParam.EFlowerType.BigBill)
        {
            return;
        }

        //ビルを成長させる
        billParam.AddExp(playerUnit.PLAYER_NO, 300.0f * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col)
    {
        GM_MathFlowerParam _billParam;

        //ビルのパラメータ取得する
        _billParam = col.GetComponent<GM_MathFlowerParam>();
        //別のオブジェクトだったら何もしない
        if (_billParam == null)
        {
            return;
        }

        //中ビルでも大ビルでも無ければ何もしない
        if (_billParam.flowerType != GM_MathFlowerParam.EFlowerType.Bill && _billParam.flowerType != GM_MathFlowerParam.EFlowerType.BigBill)
        {
            return;
        }
        //ビルとして検知する
        billParam = _billParam;
    }
    void OnTriggerExit(Collider col)
    {
        GM_MathFlowerParam _billParam;

        //ビルのパラメータ取得する
        _billParam = col.GetComponent<GM_MathFlowerParam>();
        //別のオブジェクトだったら何もしない
        if (_billParam == null)
        {
            return;
        }

        //中ビルでも大ビルでも無ければ何もしない
        if (_billParam.flowerType != GM_MathFlowerParam.EFlowerType.Bill && _billParam.flowerType != GM_MathFlowerParam.EFlowerType.BigBill)
        {
            return;
        }

        //ビル範囲外として処理する
        billParam = null;
    }

}
