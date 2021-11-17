using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    LevelData levelData = new LevelData();
    LevelData LevelData { get { return levelData; } set { levelData = value; } }
    public string path;
    public GameObject levelButtonPrefab;
    public GameObject targetTextPrefab;
    public int maxButtonInARow;//一行最多有多少按钮
    public Vector2 referencePosition;//关卡按钮阵列的参考位置（左上角）
    public Vector2 buttonInterval;//关卡按钮之间的间距


    GameObject canvas;
    GameObject mainMenu;
    GameObject targetBoard;
    List<GameObject> listButtons;
    AsyncOperation async;
    
    // Start is called before the first frame update
    void Start()
    {
        //查错
        if (maxButtonInARow < 1) maxButtonInARow = 1;
        //初始化
        canvas = GameObject.Find("Canvas");
        mainMenu = GameObject.Find("MainMenuHere");
        targetBoard = GameObject.Find("TargetBoardHere");
        targets = new List<Target>();
        listButtons = new List<GameObject>();
        LoadOnClick();

        CreateSubButtons();
    }

    IEnumerator LoadLevel(string levelName)
    {
        async = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        while(!async.isDone)
        {
            yield return null;
        }

        entities = GameObject.Find("EntityHere").GetComponentsInChildren<Entity>();
        LoadTarget("TestTarget");//test
    }

    //创建按钮阵列
    void CreateSubButtons()
    {
        int numLevel = LevelData.data.Length;
        int i, j, row = 0, index = 0;
        for (i = numLevel, j = 0; i > 0; i--)
        {
            GameObject t = Instantiate(levelButtonPrefab, canvas.transform);
            t.GetComponent<RectTransform>().anchoredPosition = referencePosition + new Vector2(buttonInterval.x * j, -buttonInterval.y * row);
            UnityEngine.UI.Button button = t.GetComponent<UnityEngine.UI.Button>();
            button.interactable = levelData.data[index].isAvailable;
            button.GetComponentInChildren<UnityEngine.UI.Text>().text = levelData.data[index].name;
            button.GetComponent<SubButton>().MarkAndBind(index, SelectLevelOnClick);
            listButtons.Add(t);

            index++;

            if (++j >= maxButtonInARow)
            {
                j = 0;
                row++;
            }
        }
    }
    //清除按钮阵列
    void ClearSubButtons()
    {
        foreach (var item in listButtons)
        {
            Destroy(item);
        }
        listButtons.Clear();
    }

    //按下对应编号按钮时的响应
    public void SelectLevelOnClick(int numMark)
    {
        //Debug.Log(numMark);

        int len = levelData.data.Length;
        if(numMark+1 < len)
        {
            levelData.data[numMark + 1].isAvailable = true;
            mainMenu.SetActive(false);
            targetBoard.SetActive(true);
            ClearSubButtons();

            StartCoroutine(LoadLevel(levelData.data[numMark].name));//test
        }
    }

    //从文件加载json配置，用于按钮
    public void LoadOnClick()
    {
        LevelData t = LoadFromFile();
        if(t != null)
            LevelData = t;
    }
    //从文件加载json
    LevelData LoadFromFile()
    {
        if (!System.IO.File.Exists(path))
            return null;
        System.IO.StreamReader sr = new System.IO.StreamReader(path);
        if (sr == null)
            return null;
        string json = sr.ReadToEnd();
        if (json.Length > 0)
            return JsonUtility.FromJson<LevelData>(json);

        return null;
    }
    //保存json配置到文件，用于按钮
    public void SaveFileOnClick()
    {
        string json = JsonUtility.ToJson(LevelData);
        System.IO.File.WriteAllText(path, json);
    }
    //输出debug信息，内容为LevelData，用于按钮
    public void PrintOnClick()
    {
        int i;
        for(i=0;i<LevelData.data.Length;i++)
        {
            Debug.Log(LevelData.data[i].name + (LevelData.data[i].isAvailable ? " t" : " f"));
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    //TargetSystem整合至此，用于管理关卡的目标
    #region
    List<Target> targets;
    Entity[] entities;

    public void LoadTarget(string targetName)
    {
        TargetData targetData = Resources.Load<TargetData>(targetName);
        Target target;
        foreach (var entity in entities)
        {
            if (entity.HaveFlagInFlags(targetData.targetFlags))
            {
                switch (targetData.type)
                {
                    case TargetData.TargetType.test:
                        target = new TestTarget { TargetData = targetData };
                        targets.Add(target);
                        entity.EventOnClick += target.Update;
                        target.EventTargetComplete += OnEventTargetComplete;
                        target.Text = Instantiate(targetTextPrefab, targetBoard.transform).GetComponent<UnityEngine.UI.Text>();
                        target.UpdateText();
                        break;
                    default:
                        Debug.LogWarning(targetData.type.ToString() + "未定义行为");
                        break;
                }
            }
        }
    }
    public void OnEventTargetComplete()
    {
        if (IsTargetAllCompleted())
            Debug.Log("allClear!!");
    }
    public bool IsTargetAllCompleted()
    {
        foreach (var target in targets)
        {
            if (!target.IsCompleted)
                return false;
        }
        return true;
    }
    #endregion
}
