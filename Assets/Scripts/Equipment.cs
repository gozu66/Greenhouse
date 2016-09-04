using UnityEngine;
using System.Collections;

public class Equipment : Item {

    public bool isMoveable;
	
	public void _Update ()
    {
        if(isMoveable)
            MoveEqupment();

    }

    public void MoveEqupment()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 mousePoint = new Vector3(hit.point.x, 0.5f, hit.point.z);
            transform.position = mousePoint;
        }

        if (Input.GetMouseButtonDown(0))
        {

            Collider[] cols = Physics.OverlapBox(transform.position, transform.localScale / 2);
            foreach (Collider col in cols)
            {
                if (col.name != "floor" && col.gameObject != gameObject)
                    return;
            }
            isMoveable = false;
            Inventory.instance.removeItem(this);
        }
    }
}
