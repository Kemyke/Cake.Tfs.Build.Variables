using Cake.Common;
using Cake.Core;
using Cake.Core.Annotations;
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Cake.Tfs.Build.Variables
{
    /// <summary>
    /// Contains functionality for accessing TFS (vnext) build variables.
    /// </summary>
    [CakeAliasCategory("TfsBuildVariables")]
    public static class TfsBuildVariablesAliases
    {
        /// <summary>
        /// This namespace contain types used for evaluating TFS build variables by name.
        /// By default TFS expose its variables as environment variables. First they convert the name of the variable
        /// to be compatible with environment variables names. But TFS doesn't resolve chained variables.
        /// This library recursively resolve chained variables and substitute all variables found in the value of
        /// other variables (see tests). 
        /// Before retreiving environment variable this library try to retrieve an Cake Argument with the variable name.
        /// This is useful because you can override values by passing it to the Cake script. And it
        /// is also useful bacuse of the secret variables. TFS doesn't expose secret variables as environment variables.
        /// You must pass secret variables as argument to the script from the TFS build step.
        /// If no argument and no environment variable found then a default value is returned if it is provided. If not
        /// an ArgumentException is thrown.
        /// </summary>
        [CompilerGenerated]
        internal class NamespaceDoc
        {
        }

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

        /// <summary>
        /// Evaluate the value of TFS variable. 
        /// Throws ArgumentException if no variable is found with the given name.
        /// </summary>
        /// <example>
        /// <code>
        /// var value = EvaluateTfsBuildVariable("Tfs.Example.Variable");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="variableName">The name of the TFS variable</param>
        /// <returns>Value of the variable.</returns>
        [CakeMethodAlias]
        public static string EvaluateTfsBuildVariable(this ICakeContext context, string variableName)
        {
            string value = EvaluateVariable(context, variableName);

            value = SubstituteNestedVariable(context, value);

            return value;
        }

        /// <summary>
        /// Evaluate the value of TFS variable. 
        /// If no variable is found with the given name it returns the default value.
        /// </summary>
        /// <example>
        /// <code>
        /// var value = EvaluateTfsBuildVariable("Tfs.Example.Variable", "defaultValue");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="variableName">The name of the TFS variable</param>
        /// <param name="defaultValue">The default value of the variable</param>
        /// <returns>Value of the variable.</returns>
        [CakeMethodAlias]
        public static string EvaluateTfsBuildVariable(this ICakeContext context, string variableName, string defaultValue)
        {
            string value = EvaluateVariable(context, variableName, defaultValue);

            if (value != null)
            {
                value = SubstituteNestedVariable(context, value);
            }

            return value;
        }
    }
}
