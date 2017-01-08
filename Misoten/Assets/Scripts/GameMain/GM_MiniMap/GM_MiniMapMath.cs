using UnityEngine;
using System.Collections;

public class GM_MiniMapMath : MonoBehaviour {
    //親のミニマップセルオブジェクト
    private GM_MiniMapCell mapCell;

    //同期対象のマス
    private GM_MathMath stageMath;

    //マス表示管理部
    private GM_MathCellColorControll colorCon;

    //マスレベルを取得
    private GM_MathFlowerParam.EFlowerLevel mathLevel;

	// Use this for initialization
	void Awake () {
        colorCon = transform.GetComponent<GM_MathCellColorControll>();
	}
    public void SetMathInfo(GM_MathMath _stageMath)
    {
        stageMath = _stageMath;
    }
    public void Init(GM_MiniMapCell _mapCell)
    {
        mapCell = _mapCell;
        colorCon.Init();
        mathLevel = stageMath.GetMinMathLevel();
    }
    public void MathStart()
    {
        //マスを表示
        colorCon.ChangeMathColor(new Color(0.5f, 0.5f, 0.5f, 1.0f), 0.0f);
    }

    //MinimapManagerから更新通知を受け取って更新
	public void UpdateMath () {
        //ステージオブジェクトがまだ未開放の場合は更新しない
        if (mapCell.stageCellObj.startFlg == false)
        {
            return;
        }

        //変更をチェック
        GM_MathFlowerParam.EFlowerLevel _mathMinLevel = stageMath.GetMinMathLevel();

        if (mathLevel != _mathMinLevel)
        {
            //レベルに更新があれば色変更処理を通す
            ChangeColor(_mathMinLevel);
        }
	}

    //レベル毎に色を分けて変更する
    private void ChangeColor(GM_MathFlowerParam.EFlowerLevel _level)
    {
        switch (_level)
        {
            case GM_MathFlowerParam.EFlowerLevel.Level0:
                colorCon.ChangeMathColor(new Color(0.5f, 0.5f, 0.5f, 1.0f), 0.3f);
                break;
            case GM_MathFlowerParam.EFlowerLevel.Level1:
                colorCon.ChangeMathColor(Color.white, 0.3f);
                break;
            case GM_MathFlowerParam.EFlowerLevel.Level2:
                colorCon.ChangeMathColor(Color.yellow, 0.3f);
                break;
            case GM_MathFlowerParam.EFlowerLevel.Level3:
                colorCon.ChangeMathColor(new Color(1.0f, 0.5f, 0.016f, 1.0f), 0.3f);
                break;
        }
    }
}
