using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;    
    
    public Sprite 
        blank, 
        weedSeed, 
        weedPkg, 
        smallPlanter;
    public Text weedListUI;

    List<ListItem> itemList;
    List<WeedData> weedList;
    public Image[] inventorySlots;

    Image currentlySelectedUISlot;


    void Awake()                                  
    {                                           
        if (instance == null)                   //Singleton Declaration
            instance = this;                    
        else if (instance != this)              
            Destroy(gameObject);                
                                                
        DontDestroyOnLoad(gameObject);          

        itemList = new List<ListItem>();
        weedList = new List<WeedData>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            foreach(ListItem i in itemList)
            {
                print(i._gameObject.name);
            }
        }

        if (weedList.Count > 0)
            weedListUI.text = weedList[0].strain + "  " + weedList[0].gramsHeld + " grams";
    }

    public void SelectUISlot(int i)
    {
        currentlySelectedUISlot = inventorySlots[i];
    }


    public Mesh m;
    public void addItem(int i)
    {
        if (itemList.Count >= inventorySlots.Length)
            return;

        GameObject go = new GameObject();
        ListItem li = new ListItem(null, null);

        switch (i)
        {
            case 0:                
                CreateWeedPlant(go, li, "African Bush");    //XML Data
                break;

            case 1:
                CreateWeedPlant(go, li, "Desert Weed");    //XML Data
                break;


            case 100:
                CreatePlanter(go, li);
                break;

        }
        SortInventorySprites();
    }

    void CreateWeedPlant(GameObject go, ListItem li, string strain)
    {
        Weed newWeed = go.AddComponent<Weed>();
        go.AddComponent<MeshRenderer>();
        go.AddComponent<BoxCollider>();
        MeshFilter mf = go.AddComponent<MeshFilter>();

        Mesh newMesh = (Mesh)Resources.Load("Art_Assets/Meshes/Temp/Plants/plantTemp1", typeof(Mesh));   //XML Mesh data
        mf.mesh = newMesh;

        newWeed.Strain = strain;            //XML Data

        li = new ListItem(go, newWeed);
        itemList.Add(li);
    }

    void CreatePlanter(GameObject go, ListItem li)
    {
        go.AddComponent<Planter>();
        go.AddComponent<MeshRenderer>();
        go.AddComponent<BoxCollider>();
        MeshFilter mf = go.AddComponent<MeshFilter>();

        Mesh newMesh = (Mesh)Resources.Load("Art_Assets/Meshes/Temp/Equipment/planterTemp1", typeof(Mesh));
        mf.mesh = newMesh;

        li = new ListItem(go, go.GetComponent<Planter>());
        itemList.Add(li);
    }

    public void AddWeed(string strain, int grams)
    {
        foreach(WeedData wd in weedList)
        {
            if(wd.strain == strain)
            {
                wd.gramsHeld += grams;
                return;
            }
        }
        WeedData reup = new WeedData(strain, grams);
        weedList.Add(reup);
    }

    void SortWeedList()
    {
        for(int i = 0; i < weedList.Count; i++)
        {
            
        }
    }

    public void SelectItem(int i)
    {
        if (itemList[i]._gameObject.GetComponent<Planter>())
        {
            itemList[i]._item.ToggleMoveable();
        }
        else if(itemList[i]._gameObject.GetComponent<Weed>())
        {
            itemList[i]._item.SeedInHand();
        }
    }

    public void removeItem(Item equipment)
    {
        foreach (ListItem x in itemList)
        {
            if (x._item == equipment)
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
                if (itemList[i]._item.GetComponent<Weed>())
                {
                    inventorySlots[i].sprite = weedSeed;
                }
                else if (itemList[i]._item.GetComponent<Planter>())
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