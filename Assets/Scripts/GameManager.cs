using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;             //Singleton Declaration
                                                    //
    void Awake()                                    //
    {                                               //
        if (instance == null)                       //
            instance = this;                        //
        else if (instance != this)                  //
            Destroy(gameObject);                    //
                                                    //
        DontDestroyOnLoad(gameObject);              //
    }

    public GameObject seedBankDotCom, growHausDotCom;
    public GameObject[] webPages;

    void Start()
    {
        webPages = new GameObject[2] { seedBankDotCom, growHausDotCom };
    }

    void Update()
    {
        GetInputs();
    }

    void GetInputs()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EnableDisableMenu(seedBankDotCom);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EnableDisableMenu(growHausDotCom);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnableDisableMenu(null);
        }
    }

    void EnableDisableMenu(GameObject selection)
    {
        foreach (GameObject page in webPages)
        {
            bool b = (page != null && page == selection) ? true : false;
            page.SetActive(b);
        }
    }
}