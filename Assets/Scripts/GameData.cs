using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* List of Equation Data for the game
 */
[System.Serializable]
public class GameData
{
    // public EquationData[] equationData;
    public EquationData tut1Equation;
    public EquationData[] type1Equations;
    public EquationData tut2Equation;
    public EquationData[] type2Equations;
    public EquationData tut3Equation;
    public EquationData[] type3Equations;
    public EquationData tut4Equation;
    public EquationData[] type4Equations;
    public EquationData tut5Equation;
    public EquationData[] type5Equations;
    public EquationData tut6Equation;
    public EquationData[] type6Equations;
    public EquationData[] type7Equations;

    public void InitializeEquationsByString()
    {
        tut1Equation.SetExpressionsByString();
        tut2Equation.SetExpressionsByString();
        tut3Equation.SetExpressionsByString();
        tut4Equation.SetExpressionsByString();
        tut5Equation.SetExpressionsByString();
        tut6Equation.SetExpressionsByString();

        for (int i = 0; i < type1Equations.Length; i++)
        {
            type1Equations[i].SetExpressionsByString();
        }

        for (int i = 0; i < type2Equations.Length; i++)
        {
            type2Equations[i].SetExpressionsByString();
        }

        for (int i = 0; i < type3Equations.Length; i++)
        {
            type3Equations[i].SetExpressionsByString();
        }

        for (int i = 0; i < type4Equations.Length; i++)
        {
            type4Equations[i].SetExpressionsByString();
        }

        for (int i = 0; i < type5Equations.Length; i++)
        {
            type5Equations[i].SetExpressionsByString();
        }

        for (int i = 0; i < type6Equations.Length; i++)
        {
            type6Equations[i].SetExpressionsByString();
        }

        for (int i = 0; i < type7Equations.Length; i++)
        {
            type7Equations[i].SetExpressionsByString();
        }
    }
}
