using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace CapFrameX.Sensor
{
    public class FormulaEvaluator
    {
        public static bool TryEvaluate(string formula, Dictionary<string, float> sensorValues, out float result)
        {
            result = 0f;
            
            if (string.IsNullOrWhiteSpace(formula))
                return false;

            try
            {
                var evaluatedFormula = ReplaceVariables(formula, sensorValues);
                
                if (string.IsNullOrWhiteSpace(evaluatedFormula))
                    return false;

                var dataTable = new DataTable();
                var computedValue = dataTable.Compute(evaluatedFormula, null);
                
                if (computedValue == null || computedValue == DBNull.Value)
                    return false;

                result = Convert.ToSingle(computedValue);
                return !float.IsNaN(result) && !float.IsInfinity(result);
            }
            catch
            {
                return false;
            }
        }

        private static string ReplaceVariables(string formula, Dictionary<string, float> sensorValues)
        {
            var result = formula;
            
            var variablePattern = @"\[([^\]]+)\]";
            var matches = Regex.Matches(formula, variablePattern);
            
            foreach (Match match in matches)
            {
                var sensorName = match.Groups[1].Value;
                var matchingKey = sensorValues.Keys.FirstOrDefault(k => 
                    k.IndexOf(sensorName, StringComparison.OrdinalIgnoreCase) >= 0);
                
                if (matchingKey != null)
                {
                    result = result.Replace(match.Value, sensorValues[matchingKey].ToString("G", System.Globalization.CultureInfo.InvariantCulture));
                }
                else
                {
                    return null;
                }
            }
            
            return result;
        }

        public static List<string> ExtractSensorReferences(string formula)
        {
            if (string.IsNullOrWhiteSpace(formula))
                return new List<string>();

            var variablePattern = @"\[([^\]]+)\]";
            var matches = Regex.Matches(formula, variablePattern);
            
            return matches.Cast<Match>()
                .Select(m => m.Groups[1].Value)
                .Distinct()
                .ToList();
        }
    }
}
