using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;    
    
    public Sprite blank, weedSeed, weedPkg, smallPlanter;

    List<ListItem> itemList;
    List<WeedData> weedList;
    
    Dictionary<string, Text> weedListUI = new Dictionary<string, Text>();

    public Image[] inventorySlots;
    Image currentlySelectedUISlot;

    VerticalLayoutGroup weedListVLayout;

    public Text UiCash;

    int playerCash;
    public int PlayerCash
    {
        get {
            return playerCash;
        }
        set {
            print("Cash was Set");
            playerCash = value;
        }
    }
    
    void Awake()
    {
        if (instance == null)                   //Singleton Declaration
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {                                              
        itemList = new List<ListItem>();
        weedList = new List<WeedData>();

        weedListVLayout = FindObjectOfType<VerticalLayoutGroup>();
        UiCash = GameObject.FindWithTag("CashUI").GetComponent<Text>();

        SetCash(100);
    }

    void SetCash(int cash)
    {
        int i = PlayerCash += cash;
        UiCash.text = "$" + i.ToString();
    }

    public void SelectUISlot(int i)
    {
        currentlySelectedUISlot = inventorySlots[i];
    }

    public void addItem(int i)
    {
        if (itemList.Count >= inventorySlots.Length)
            return;

        GameObject go = new GameObject();
        ListItem li = new ListItem(null, null);

        switch (i)
        {
            case 0:
                CreateObject(go, li, "African Bush");    //XML Data
                break;

            case 1:
                CreateObject(go, li, "Desert Weed");    //XML Data
                break;


            case 100:
                CreateObject(go, li, "NOT_WEED");
                break;

        }
        SortInventorySprites();
    }

    void CreateObject(GameObject go, ListItem li, string strain)
      {
          Mesh newMesh;
          Weed newWeed = null;
          Material newMat;

          if (strain != "NOT_WEED")
          {
              newWeed = go.AddComponent<Weed>();
              newWeed.Strain = strain;            
              newMesh = (Mesh)Resources.Load("Art_Assets/Meshes/Temp/Plants/plantTemp1", typeof(Mesh));           //XML Mesh data
              newMat = (Material)Resources.Load("Materials/Proto/Plant");
          }
          else
          {
              go.AddComponent<Planter>();
              newMesh = (Mesh)Resources.Load("Art_Assets/Meshes/Temp/Equipment/planterTemp1", typeof(Mesh));      //XML Mesh data
              newMat = (Material)Resources.Load("Materials/Proto/Door");
          }

          MeshRenderer mr = go.AddComponent<MeshRenderer>();
          BoxCollider bc = go.AddComponent<BoxCollider>();
          MeshFilter mf = go.AddComponent<MeshFilter>();

          Bounds newMeshBounds = newMesh.bounds;
          mf.mesh = newMesh;
          float offset = newMeshBounds.size.z / 2;
          bc.center = new Vector3(bc.center.x, bc.center.y, bc.center.z + offset);
          bc.size = newMeshBounds.extents * 2;        

          go.transform.Rotate(new Vector3(270, 0, 0));
          mr.sharedMaterial = newMat;

          if (strain != "NOT_WEED")
          {
            li = new ListItem(go, newWeed);
            itemList.Add(li);        
          }
          else
          {
            li = new ListItem(go, go.GetComponent<Planter>());
            itemList.Add(li);
          }
      }

    public void AddWeed(string strain, int grams)
    {
        foreach(WeedData wd in weedList)
        {
            if(wd.strain == strain)
            {
                int newGrams = wd.gramsHeld += grams;
                Text t;
                if (weedListUI.TryGetValue(strain, out t))
                {
                    t.text = strain + " " + newGrams.ToString() + " g";
                }
                return;
            }
        }
        WeedData reup = new WeedData(strain, grams);
        weedList.Add(reup);

        Text newWeedUI = NewWeedUI(strain, grams);        
        weedListUI.Add(strain, newWeedUI);
    }

    Text NewWeedUI(string strain, int grams)
    {
        GameObject go = new GameObject();
        go.AddComponent<CanvasRenderer>();
        go.AddComponent<RectTransform>();
        Text t = go.AddComponent<Text>();
        Font _font = (Font)Resources.Load("Fonts/coolvetica rg");
        t.font = _font;
        t.text = strain + " " + grams + " g";
        t.color = Color.black;
        t.transform.SetParent(weedListVLayout.transform, false);
        return t;
    }

    public void SelectItem(int i)
    {
        if (i >= itemList.Count)
            return;

        if (itemList[i]._gameObject.GetComponent<Planter>())
        {
            itemList[i]._gameObject.GetComponent<Planter>().ToggleMoveable();
        }
        else if(itemList[i]._gameObject.GetComponent<Weed>())
        {
            itemList[i]._item.SeedInHand();
        }
    }

    public void removeItem(Item equipment)
    {
        foreach(ListItem x in itemList)
        {
            if(x._item == equipment)
            {
                itemList.Remove(x);
                SortInventorySprites();
                return;
            }
        }
    }

    public void SortInventorySprites()
    {
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            if(i < itemList.Count)
            {
                if (itemList[i]._gameObject.GetComponent<Weed>())
                {
                    inventorySlots[i].sprite = weedSeed;
                }
                else if (itemList[i]._gameObject.GetComponent<Planter>())
                {
                    inventorySlots[i].sprite = smallPlanter;
                }
            }
            else
            {
                inventorySlots[i].sprite = blank;
            }
        }
    }
}

public class ListItem
{
    public Item _item;
    public GameObject _gameObject;

    public ListItem(GameObject go, Item it)
    {
        _item = it;
        _gameObject = go;
    }
}

public class WeedData
{
    public string strain;
    public int gramsHeld;

    public WeedData(string strain, int gramsHeld)
    {
        this.strain = strain;
        this.gramsHeld = gramsHeld;
    }
}