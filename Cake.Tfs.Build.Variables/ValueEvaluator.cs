using Cake.Common;
using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Text.RegularExpressions;

namespace Cake.Tfs.Build.Variables
{
    public static class ValueEvaluator
    {
        private static string EnvNameFromVariableName(string variableName)
        {
            return variableName.ToUpper().Replace(".", "_");
        }

        private static string EvaluateVariable(ICakeContext context, string variableName)
        {
            return EvaluateVariable(context, variableName, false, null);
        }

        private static string EvaluateVariable(ICakeContext context, string variableName, string defaultValue)
        {
            return EvaluateVariable(context, variableName, true, defaultValue);
        }

        private static string EvaluateVariable(ICakeContext context, string variableName, bool hasDefaultValue, string defaultValue)
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
                if (hasDefaultValue)
                {
                    return defaultValue;
                }
                else
                {
                    throw new ArgumentException($"No argument found with name {variableName} and no environment variable found with name {envVariablename}!");
                }
            }

            return value;
        }

        private static string SubstituteNestedVariable(ICakeContext context, string value)
        {
            string variablepattern = @"\$\(([a-zA-Z0-9_.]*)\)";
            var match = Regex.Match(value, variablepattern);

            while (match.Success)
            {
                string innerVarValue = EvaluateVariable(context, match.Groups[1].Value);

                value = value.Replace(match.Value, innerVarValue);
                match = Regex.Match(value, variablepattern);
            }

            return value;
        }

        [CakeMethodAlias]
        public static string EvaluateTfsBuildVariable(this ICakeContext context, string variableName)
        {
            string value = EvaluateVariable(context, variableName);

            value = SubstituteNestedVariable(context, value);

            return value;
        }

        [CakeMethodAlias]
        public static string EvaluateTfsBuildVariable(this ICakeContext context, string variableName, string defaultValue)
        {
            string value = EvaluateVariable(context, variableName, defaultValue);

            value = SubstituteNestedVariable(context, value);

            return value;
        }
    }
}
