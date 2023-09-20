using System;
using UnityEngine;

public class FormulaReader : MonoBehaviour
{
    public int hp;
    public int curWave;

    void Start()
    {
        string formula = "hp * (curWave * 2)*32+123";

        // Replace variable names with actual values in the formula
        formula = SubstituteVariables(formula);

        // Evaluate the formula
        int result = EvaluateFormula(formula);

        // Output the result
        Debug.Log($"Result: {result}");
    }

    string SubstituteVariables(string formula)
    {
        formula = formula.Replace("hp", hp.ToString());
        formula = formula.Replace("curWave", curWave.ToString());
        return formula;
    }

    int EvaluateFormula(string formula)
    {
        try
        {
            object evalResult = eval(formula);
            if (evalResult != null)
            {
                return Convert.ToInt32(evalResult);
            }
            else
            {
                return 0;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error evaluating formula: " + ex.Message);
            return 0; // Handle the error as needed
        }
    }

    object eval(string expression)
    {
        System.Data.DataTable table = new System.Data.DataTable();
        table.Columns.Add("expression", typeof(string), expression);
        System.Data.DataRow row = table.NewRow();
        table.Rows.Add(row);
        return row["expression"];
    }
}