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
   // public Text weedListUI;

    List<ListItem> itemList;
    List<WeedData> weedList;
    public Image[] inventorySlots;

    Image currentlySelectedUISlot;

    List <Text> weedListUI;
    VerticalLayoutGroup vLayout;

    void Awake()                                  
    {                                           
        if (instance == null)                   //Singleton Declaration
            instance = this;                    
        else if (instance != this)              
            Destroy(gameObject);                
                                                
        DontDestroyOnLoad(gameObject);          

        itemList = new List<ListItem>();
        weedList = new List<WeedData>();

        vLayout = FindObjectOfType<VerticalLayoutGroup>();
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            foreach(ListItem i in itemList)
            {
                print(i._gameObject.name);//i.strain + " "+i.gramsHeld+" grams");
            }
            Debug.Log(itemList.Count);

        }

       // if (weedList.Count > 0)
           // weedListUI.text = weedList[0].strain + "  " + weedList[0].gramsHeld + " grams";
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

    /*   void CreateWeedPlant(GameObject go, ListItem li, string strain)
       {
           Weed newWeed = go.AddComponent<Weed>();
           MeshRenderer mr = go.AddComponent<MeshRenderer>();
           BoxCollider bc = go.AddComponent<BoxCollider>();
           MeshFilter mf = go.AddComponent<MeshFilter>();

           Mesh newMesh = (Mesh)Resources.Load("Art_Assets/Meshes/Temp/Plants/plantTemp1", typeof(Mesh));   //XML Mesh data
           Bounds newMeshBounds = newMesh.bounds;
           mf.mesh = newMesh;
           float offset = newMeshBounds.size.z / 2;
           bc.center = new Vector3(bc.center.x, bc.center.y, bc.center.z + offset);
           bc.size = newMeshBounds.extents * 2;

           newWeed.Strain = strain;            //XML Data

           go.transform.Rotate(new Vector3(270, 0, 0));

           mr.sharedMaterial = (Material)Resources.Load("Materials/Proto/Plant");

           li = new ListItem(go, newWeed);
           itemList.Add(li);
       }

       void CreatePlanter(GameObject go, ListItem li)
       {
           go.AddComponent<Planter>();
           MeshRenderer mr = go.AddComponent<MeshRenderer>();
           BoxCollider bc = go.AddComponent<BoxCollider>();
           MeshFilter mf = go.AddComponent<MeshFilter>();

           Mesh newMesh = (Mesh)Resources.Load("Art_Assets/Meshes/Temp/Equipment/planterTemp1", typeof(Mesh));
           Bounds newMeshBounds = newMesh.bounds;
           mf.mesh = newMesh;
           float offset = newMeshBounds.size.z / 2;
           bc.center = new Vector3(bc.center.x, bc.center.y, bc.center.z + offset);
           bc.size = newMeshBounds.extents * 2;

           go.transform.Rotate(new Vector3(270, 0, 0));
           mr.sharedMaterial = (Material)Resources.Load("Materials/Proto/Door");

           li = new ListItem(go, go.GetComponent<Planter>());
           itemList.Add(li);
       }
       */

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
                wd.gramsHeld += grams;
                return;
            }
        }
        WeedData reup = new WeedData(strain, grams);
        weedList.Add(reup);

        Text newWeedUI = NewWeedUI(strain, grams);        
        newWeedUI.transform.SetParent(vLayout.transform, false);
        newWeedUI.color = Color.black;
        Font _font = (Font)Resources.Load("Fonts/coolvetica rg");
        newWeedUI.font = _font;                
    }

    void UpdateWeedUI()
    {

    }

    Text NewWeedUI(string strain, int grams)
    {
        GameObject go = new GameObject();
        go.AddComponent<CanvasRenderer>();
        go.AddComponent<RectTransform>();
        Text t = go.AddComponent<Text>();
        t.text = strain + " " + grams + " grams";
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