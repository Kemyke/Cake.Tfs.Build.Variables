using Cake.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System.Runtime.Versioning;
using Cake.Core.Configuration;
using System.Linq;
using System.Xml.Linq;

namespace Cake.Tfs.Build.Variables.Tests
{
    public class TestCakeContext : ICakeContext
    {
        public TestCakeContext() { Arguments = new TestArguments(); Environment = new TestEnvironment(); }
        public TestCakeContext(Dictionary<string, string> envVars, Dictionary<string, string> arguments) { Environment = new TestEnvironment(envVars); Arguments = new TestArguments(arguments); }

        public IFileSystem FileSystem => throw new NotImplementedException();

        public ICakeEnvironment Environment { get; private set; }

        public IGlobber Globber => throw new NotImplementedException();

        public ICakeLog Log => throw new NotImplementedException();

        public ICakeArguments Arguments { get; private set; }

        public IProcessRunner ProcessRunner => throw new NotImplementedException();

        public IRegistry Registry => throw new NotImplementedException();

        public IToolLocator Tools => throw new NotImplementedException();

        public ICakeDataResolver Data => throw new NotImplementedException();

        public ICakeConfiguration Configuration => throw new NotImplementedException();
    }

    public class TestArguments : ICakeArguments
    {
        private Dictionary<string, string> arguments = new Dictionary<string, string>();

        public TestArguments() { }
        public TestArguments(Dictionary<string, string> arguments) { this.arguments = arguments; }

        public string GetArgument(string name)
        {
            return arguments[name];
        }

        public ICollection<string> GetArguments(string name)
        {
            return new List<string> { arguments[name] };

        }

        public IDictionary<string, ICollection<string>> GetArguments()
        {
            return arguments.ToDictionary(k => k.Key, v => (ICollection<string>)new List<string> { v.Value });
        }

        public bool HasArgument(string name)
        {
            return arguments.ContainsKey(name);
        }
    }

    public class TestEnvironment : ICakeEnvironment
    {
        private Dictionary<string, string> envVars = new Dictionary<string, string>();

        public DirectoryPath WorkingDirectory { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public DirectoryPath ApplicationRoot => throw new NotImplementedException();

        public ICakePlatform Platform => throw new NotImplementedException();

        public ICakeRuntime Runtime => throw new NotImplementedException();

        public TestEnvironment() { }
        public TestEnvironment(Dictionary<string, string> envVars) { this.envVars = envVars; }

        public DirectoryPath GetSpecialPath(SpecialPath path)
        {
            throw new NotImplementedException();
        }

        public string GetEnvironmentVariable(string variable)
        {
            string value;
            if(envVars.TryGetValue(variable, out value))
            {
                return value;
            }
            return null;
        }

        public IDictionary<string, string> GetEnvironmentVariables()
        {
            return envVars;
        }

        public bool Is64BitOperativeSystem()
        {
            throw new NotImplementedException();
        }

        public bool IsUnix()
        {
            throw new NotImplementedException();
        }

        public DirectoryPath GetApplicationRoot()
        {
            throw new NotImplementedException();
        }

        public FrameworkName GetTargetFramework()
        {
            throw new NotImplementedException();
        }
    }


}
