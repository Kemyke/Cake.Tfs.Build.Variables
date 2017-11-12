using Cake.Common;
using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Text.RegularExpressions;

namespace Cake.Tfs.VNext.Variables
{
    public static class ValueEvaluator
    {
        private static string EnvNameFromVariableName(string variableName)
        {
            return variableName.ToUpper().Replace(".", "_");
        }

        private static string GetVnextVariableValue(ICakeContext context, string variableName)
        {
            string envVariablename = EnvNameFromVariableName(variableName);

            string value;
            if (context.HasArgument(variableName))
            {
                value = context.Argument<string>(variableName);
            }
            else if (context.HasEnvironmentVariable(envVariablename))
            {
                value = context.EnvironmentVariable(envVariablename);
            }
            else
            {
                throw new ArgumentException($"No argument found with name {variableName} and no environment variable found with name {envVariablename}!");
            }

            return value;
        }

        private static string SubstituteNestedVariable(ICakeContext context, string value)
        {
            string variablepattern = @"\$\(([a-zA-Z0-9_.]*)\)";
            var match = Regex.Match(value, variablepattern);

            while (match.Success)
            {
                string innerVarValue = GetVnextVariableValue(context, match.Groups[1].Value);

                value = value.Replace(match.Value, innerVarValue);
                match = Regex.Match(value, variablepattern);
            }

            return value;
        }

        [CakeMethodAlias]
        public static string EvaluateVnextVariable(this ICakeContext context, string variableName)
        {
            string value = GetVnextVariableValue(context, variableName);

            value = SubstituteNestedVariable(context, value);

            return value;
        }
    }
}
