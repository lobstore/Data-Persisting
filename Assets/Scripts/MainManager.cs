using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
public class MainManager : MonoBehaviour
{
    static public MainManager Instance;
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public Text ScoreText;
    public Text bestScore;
    public GameObject GameOverText;
    public InputField playerName;
    private bool m_Started = false;
    private int m_Points;
    public Button submit;
    private bool m_GameOver = false;
    static private int b_points;
    static private string b_name;
    private string m_name;
    public Paddle paddlePlayer;

    [System.Serializable]
    public class SaveClass
    {
       public int b_points;
       public string b_name;
    }
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveClass data = JsonUtility.FromJson<SaveClass>(json);

            b_points = data.b_points;
            b_name = data.b_name;
        }
        Debug.Log(Application.persistentDataPath);
        m_Points = 0;
        Time.timeScale = 0;
        paddlePlayer = paddlePlayer.gameObject.GetComponent<Paddle>();
        paddlePlayer.enabled = false;
        AddPoint(m_Points);
    }
    public void SubmitName()
    {
        Time.timeScale = 1;
        m_name = playerName.text;
        submit.gameObject.SetActive(false);
        playerName.gameObject.SetActive(false);
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
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
        paddlePlayer.enabled = true;
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
            Time.timeScale = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {

        m_Points += point;
        if (m_Points>b_points)
        {
            b_points = m_Points;
            b_name = m_name;
        }
        ScoreText.text = $"Score : {m_Points}";
        bestScore.text = $"Best Score :{b_points} Name : {b_name}";
    }

    public void GameOver()
    {
        SaveClass save = new SaveClass();
        save.b_name = b_name;
        save.b_points = b_points;
        string json = JsonUtility.ToJson(save);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
