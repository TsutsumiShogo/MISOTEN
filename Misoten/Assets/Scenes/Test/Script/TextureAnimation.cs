using UnityEngine;
using System.Collections;

public class TextureAnimation : MonoBehaviour {

    public Sprite[] m_sprites;          // スプライト
    public float m_intervalSecond;      // 間隔
    public bool m_loop = false;
    private float m_dTime;  // デルタタイム計測
    private int m_frameNum; // Frame番号

	// Use this for initialization
	void Start () {
        m_dTime = 0.0f;
        m_frameNum = 0;
	}
	
	// Update is called once per frame
	void Update () {
      
        m_dTime += Time.deltaTime;
        if (m_intervalSecond < m_dTime){
            m_dTime = 0.0f;
            m_frameNum++;
            if (m_frameNum >= m_sprites.Length) m_frameNum = 0;
            GetComponent<SpriteRenderer>().sprite = m_sprites[m_frameNum];
        }
        
	}
}
