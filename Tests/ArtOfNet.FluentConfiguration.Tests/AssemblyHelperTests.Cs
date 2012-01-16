using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ArtOfNet.ConfArt.Core;
using ConfArt.Contracts;

namespace ConfArtTests
{
    [TestFixture]
    public class AssemblyHelperTests: TestFixtureBase
    {
        [Test]
        public void VerifyUserLoadedAssemblies()
        {
            var assemblies = AssemblyHelper.GetUserLoadedAssemblies();
            var names = from assembly in assemblies
                        select assembly.GetName().Name;

            Log(names);

            Assert.Greater(assemblies.Count(), 0);
            //the current test assembly should be loaded
            Assert.Contains("ConfArtTests", names.ToList());
            //the assembly to test should also be correctly loaded
            Assert.Contains("ArtOfNet.ConfArt", names.ToList());
        }

        [Test]
        public void VerifyIfCanGetConfigurationFromUserLoadedAssemblies()
        {
            IEnumerable<Tuple<Type, Type>> loaded = AssemblyHelper.GetAllGenericTypesFromUserLoadedAssemblies();
            var interfaces = from t in loaded
                             select t.Item2.Name;
            Log(loaded);
            Assert.DoesNotThrow(() => { loaded.Count(); },"Should not throw an error when counting the loaded user assemblies");
            Assert.Contains(typeof(IConfigurationMappingsBase<>).Name, interfaces.ToList(),"you should have loaded the IConfigurationMappingsBase interface");
        }

        [Test]
        public void VerifyIfCanGetConfigurationMappings()
        {
            IEnumerable<ConfigurationMappingType> loaded = AssemblyHelper.GetConfigurationMappings();
            var sources = from t in loaded
                             select t.Source.Name;
            var targets = from t in loaded
                          select t.Target.Name;

            Log(sources,"Sources:");
            Log(targets, "Targets:");

            Assert.DoesNotThrow(() => { loaded.Count(); }, "Should not throw an error when counting the loaded user assemblies");
            Assert.Contains("User", sources.ToList(),"Loaded types should contains the 'User' class");
            Assert.Contains("UserMap", targets.ToList(), "Loaded types shoudl contains the 'UserMap' class used to define 'User' class mappings");
        }
       
    }
}
