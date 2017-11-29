using System;
using System.Collections.Generic;
using Xunit;

namespace Cake.Tfs.Build.Variables.Tests
{
    public class ValueEvaluatorTests
    {
        [Fact]
        public void NonExistentValueTest()
        {
            try
            {
                var c = new TestCakeContext();
                ValueEvaluator.EvaluateTfsBuildVariable(c, "Cake.Variables.Test");
                Assert.True(false);
            }
            catch (ArgumentException)
            {
                Assert.True(true);
            }
        }

        [Fact]
        public void NonExistentValueWithDefaultValueTest()
        {
            var c = new TestCakeContext();
            var value = ValueEvaluator.EvaluateTfsBuildVariable(c, "Cake.Variables.Test", "defval");
            Assert.Equal("defval", value);
        }

        [Fact]
        public void ValueFromEnvTest()
        {
            Dictionary<string, string> envVars = new Dictionary<string, string>() { { "CAKE_VARIABLES_TEST", "testenvvalue" } };
            Dictionary<string, string> args = new Dictionary<string, string>();

            var c = new TestCakeContext(envVars, args);
            var value = ValueEvaluator.EvaluateTfsBuildVariable(c, "Cake.Variables.Test");
            Assert.Equal("testenvvalue", value);
        }

        [Fact]
        public void ValueFromArgTest()
        {
            Dictionary<string, string> envVars = new Dictionary<string, string>();
            Dictionary<string, string> args = new Dictionary<string, string>() { { "Cake.Variables.Test", "testargvalue" } };

            var c = new TestCakeContext(envVars, args);
            var value = ValueEvaluator.EvaluateTfsBuildVariable(c, "Cake.Variables.Test");
            Assert.Equal("testargvalue", value);
        }

        [Fact]
        public void ValueFromEnvAndArgTest()
        {
            Dictionary<string, string> envVars = new Dictionary<string, string>() { { "CAKE_VARIABLES_TEST", "testenvvalue" } };
            Dictionary<string, string> args = new Dictionary<string, string>() { { "Cake.Variables.Test", "testargvalue" } };

            var c = new TestCakeContext(envVars, args);
            var value = ValueEvaluator.EvaluateTfsBuildVariable(c, "Cake.Variables.Test");
            Assert.Equal("testargvalue", value);
        }

        [Fact]
        public void ValueFromEnvAndArgWithDefaultValueTest()
        {
            Dictionary<string, string> envVars = new Dictionary<string, string>() { { "CAKE_VARIABLES_TEST", "testenvvalue" } };
            Dictionary<string, string> args = new Dictionary<string, string>() { { "Cake.Variables.Test", "testargvalue" } };

            var c = new TestCakeContext(envVars, args);
            var value = ValueEvaluator.EvaluateTfsBuildVariable(c, "Cake.Variables.Test", "defval");
            Assert.Equal("testargvalue", value);
        }
    }
}
