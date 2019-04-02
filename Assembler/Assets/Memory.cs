using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    public string name = "";
    public int data = 0;

    public void UpdateData()
    {
        if (name.Length > 0)
        {
            int.TryParse(name, out data);
            if (name.Substring(0, 1) == "#")
            {
                data = int.Parse(name.Substring(1, name.Length - 1));
            }

            

            

        }
    }

}
