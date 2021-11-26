using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public Text BestResult;
    public Text ScoreText;
    public GameObject GameOverText;
    public InputField nname;
    private bool m_Started = false;
    private int m_Points;
    private int b_Points=0;
    private bool m_GameOver = false;
    private string b_name;
    private string m_name;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        m_Points = 0;
        Time.timeScale = 0;
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }
    public void SubmitName()
    {
        m_name = nname.text;
        nname.gameObject.SetActive(false);
        button.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        
        if (m_Points > b_Points)
        {
            b_Points = m_Points;
            b_name = m_name;
            BestResult.text = $"Best Score: {b_Points} Name: {b_name}";
        }
           BestResult.text = $"Best Score: {b_Points} Name: {b_name}";
        ScoreText.text = $"Score : {b_Points}";
        
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
