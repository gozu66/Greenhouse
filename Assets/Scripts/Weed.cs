using UnityEngine;
using System.Collections;
using System;

public class Weed : Item
{
    string strain;
    public string Strain
    {
        get
        {
            return strain;
        }
        set
        {
            strain = value;
        }
    }
    
    
    float growTime = 3.0f;          //XML DATA
    int maxHarvest, 
        minHarvest,
        currentHarvest;


    bool inHand = false;            //SCRIPT DATA
    bool readyToHarvest;

    float timeSincePlanted;
    float timeSincePlantedModifier;

    int interval = 2;


    void Update()
    {
        if (Input.GetMouseButtonDown(0) && inHand)
        {
            if(inHand)
            {
                PlantSeed();                
            }
        }
    }

    void PlantSeed()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.GetComponent<Planter>())
            {
                inHand = false;
                Inventory.instance.removeItem(this);
                timeSincePlantedModifier = Time.timeSinceLevelLoad;
                transform.position = hit.transform.position;
                InvokeRepeating("GrowTicker", interval, interval);
            }
        }
    }
        

    public override void SeedInHand()
    {
        inHand = true;
    }

    void GrowTicker()
    {
        timeSincePlanted = Time.timeSinceLevelLoad - timeSincePlantedModifier;
        currentHarvest = CalculateGrow();

        if (timeSincePlanted >= growTime)
        {
            readyToHarvest = true;
            CancelInvoke("GrowTicker");
            print("done");

        }
        else
        {
            print("tick");
        }
    }

    int CalculateGrow()
    {
        //Check for attatchement buffs
        //check for debuffs

        //Assign new harvest amount
        currentHarvest++;

        return currentHarvest;
    }

    void OnMouseDown()
    {
        if (readyToHarvest)
        {
            Inventory.instance.AddWeed(Strain, currentHarvest);
            //this.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}