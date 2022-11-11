using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Cake.Tfs.Build.Variables.Tests
{
    [TestClass]
    public class ValueEvaluatorTests
    {
        [TestMethod]
        public void NonExistentValueTest()
        {
            try
            {
                var c = new TestCakeContext();
                TfsBuildVariablesAliases.EvaluateTfsBuildVariable(c, "Cake.Variables.Test");
                Assert.IsTrue(false);
            }
            catch (ArgumentException)
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void NonExistentValueWithDefaultValueTest()
        {
            var c = new TestCakeContext();
            var value = TfsBuildVariablesAliases.EvaluateTfsBuildVariable(c, "Cake.Variables.Test", "defval");
            Assert.AreEqual("defval", value);
        }

        [TestMethod]
        public void NonExistentValueWithNullDefaultValueTest()
        {
            var c = new TestCakeContext();
            var value = TfsBuildVariablesAliases.EvaluateTfsBuildVariable(c, "Cake.Variables.Test", null);
            Assert.IsNull(value);
        }

        [TestMethod]
        public void ValueFromEnvTest()
        {
            Dictionary<string, string> envVars = new Dictionary<string, string>() { { "CAKE_VARIABLES_TEST", "testenvvalue" } };
            Dictionary<string, string> args = new Dictionary<string, string>();

            var c = new TestCakeContext(envVars, args);
            var value = TfsBuildVariablesAliases.EvaluateTfsBuildVariable(c, "Cake.Variables.Test");
            Assert.AreEqual("testenvvalue", value);
        }

        [TestMethod]
        public void ValueFromArgTest()
        {
            Dictionary<string, string> envVars = new Dictionary<string, string>();
            Dictionary<string, string> args = new Dictionary<string, string>() { { "Cake.Variables.Test", "testargvalue" } };

            var c = new TestCakeContext(envVars, args);
            var value = TfsBuildVariablesAliases.EvaluateTfsBuildVariable(c, "Cake.Variables.Test");
            Assert.AreEqual("testargvalue", value);
        }

        [TestMethod]
        public void NewFormValueFromArgTest()
        {
            Dictionary<string, string> envVars = new Dictionary<string, string>();
            Dictionary<string, string> args = new Dictionary<string, string>() { { "Cake_Variables_Test", "testargvalue" } };

            var c = new TestCakeContext(envVars, args);
            var value = TfsBuildVariablesAliases.EvaluateTfsBuildVariable(c, "Cake.Variables.Test");
            Assert.AreEqual("testargvalue", value);
        }

        [TestMethod]
        public void ValueFromEnvAndArgTest()
        {
            Dictionary<string, string> envVars = new Dictionary<string, string>() { { "CAKE_VARIABLES_TEST", "testenvvalue" } };
            Dictionary<string, string> args = new Dictionary<string, string>() { { "Cake.Variables.Test", "testargvalue" } };

            var c = new TestCakeContext(envVars, args);
            var value = TfsBuildVariablesAliases.EvaluateTfsBuildVariable(c, "Cake.Variables.Test");
            Assert.AreEqual("testargvalue", value);
        }

        [TestMethod]
        public void ValueFromEnvAndArgWithDefaultValueTest()
        {
            Dictionary<string, string> envVars = new Dictionary<string, string>() { { "CAKE_VARIABLES_TEST", "testenvvalue" } };
            Dictionary<string, string> args = new Dictionary<string, string>() { { "Cake.Variables.Test", "testargvalue" } };

            var c = new TestCakeContext(envVars, args);
            var value = TfsBuildVariablesAliases.EvaluateTfsBuildVariable(c, "Cake.Variables.Test", "defval");
            Assert.AreEqual("testargvalue", value);
        }

        [TestMethod]
        public void ChainedValueTest()
        {
            Dictionary<string, string> envVars = new Dictionary<string, string>() { { "CAKE_VARIABLES_TEST", "Test value: $(Cake.Variables.MoreTest)." }, { "CAKE_VARIABLES_MORETEST", "12345" } };
            Dictionary<string, string> args = new Dictionary<string, string>();

            var c = new TestCakeContext(envVars, args);
            var value = TfsBuildVariablesAliases.EvaluateTfsBuildVariable(c, "Cake.Variables.Test");
            Assert.AreEqual("Test value: 12345.", value);
        }
    }
}
