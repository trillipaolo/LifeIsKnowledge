using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ComboGridDimension")]
public class ComboGridDimensions : ScriptableObject {

    [SerializeField]
    private int rows;

    [SerializeField]
    private int coloumns;

    public int GetRows()
    {
        return rows;
    }

    public int GetColoumns()
    {
        return coloumns;
    }

    public void AddRows(int add)
    {
        if (add > 0)
        {
            int newRows = rows + add;
            if (newRows < 6 && newRows > 0)
            {
                rows = newRows;
            }
        }
    }

    public void AddColoumns(int add)
    {
        if (add > 0)
        {
            int newColoumns = coloumns + add;
            if (newColoumns < 6 && newColoumns > 0)
            {
                coloumns = newColoumns;
            }
        }
    }

}
