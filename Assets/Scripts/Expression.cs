using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Expression
{
    public int numVars;
    public int numValues;
    public int numBrackets;
    public int[] bracketCoefficients;
    public Expression[] bracketExpressions;
    public string representation; 
}
