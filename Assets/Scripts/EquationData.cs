using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* EquationData represents one equation that will be solved by the player
 * during the level.
 */
[System.Serializable]
public class EquationData
{
    // public int timeLimit;
    public int difficulty;
    public int variableValue;
    public Expression lhs;
    public Expression rhs;
    public string equation; 

	public void SetExpressionsByString()
	{		
		string[] sides = equation.Split(new string[] { " = " }, StringSplitOptions.None);
		string left = sides[0];
		string right = sides[1];

		lhs = StringToExpression(left);
        rhs = StringToExpression(right);
	}

    public Expression StringToExpression(string exp)
	{
		Expression expression = new Expression();
		
		string[] terms = SplitIntoTerms(exp);
		
		int NumVars = 0;
		int NumValues = 0;
		int NumBrackets = 0;
		List<int> BracketCoefficients = new List<int>();
		List<Expression> BracketExpressions = new List<Expression>();
		
		for (int i = 0; i < terms.Length; i++)
		{
			Console.WriteLine(terms[i]);
			if (terms[i].Contains("("))
			{
				string coef = terms[i].Substring(0, terms[i].IndexOf("("));
                if (coef.Length == 0)
				{
					coef = "1";
				}
				if (coef == "-")
				{
					coef = "-1";
				}

				NumBrackets = NumBrackets + 1;
				BracketCoefficients.Add(Int32.Parse(coef));
				string partone = terms[i].Substring(terms[i].IndexOf("(") + 1);
				BracketExpressions.Add(StringToExpression(partone.Substring(0, partone.Length - 1)));
				
			}
			else if (terms[i].Contains("x"))
			{
				string coef = terms[i].Substring(0, terms[i].Length - 1);
				if (coef.Length == 0)
				{
					coef = "1";
				}
				if (coef == "-")
				{
					coef = "-1";
				}
				NumVars = NumVars + Int32.Parse(coef);
			}
			else
			{
				NumValues = NumValues + Int32.Parse(terms[i]);
			}
		}
		
		expression.numValues = NumValues;
		expression.numVars = NumVars;
		expression.numBrackets = NumBrackets;
		expression.bracketCoefficients = BracketCoefficients.ToArray();
		expression.bracketExpressions = BracketExpressions.ToArray();
		
		return expression;
	}
	
	public string[] SplitIntoTerms(string stringExpression)
	{
		string[] express = stringExpression.Split(' ');
		List<string> termsList = new List<string>();

		string current = "";
		bool inbracket = false;
		for (int check = 0; check < express.Length; check++)
		{
			if (express[check].Contains("(") || inbracket)
			{
				inbracket = true;
				current = current + " " + express[check];
				
				if (express[check].Contains(")"))
				{
					inbracket = false;
					termsList.Add(current.Trim());
					current = "";
				}
			}
			else
			{
				termsList.Add(express[check]);
			}
		}
		
		List<string> terms = new List<string>();
		
		string curr = "";
		int i = 0;
		while (i < termsList.Count)
		{
			if (i > 0)
			{
				if (termsList[i - 1] == "-")
				{
					curr = "-";
				}
			}
			curr = curr + termsList[i];
			terms.Add(curr);
			i = i + 2;
			curr = "";
		}
		
		return terms.ToArray();
	}

}
