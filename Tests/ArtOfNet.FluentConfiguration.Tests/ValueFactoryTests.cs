using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ConfArt.Core;
using ConfArt;
using ConfArtTests.SampleData;
using ConfArt.Contracts;

namespace ConfArtTests
{
    [TestFixture]
    public class ValueFactoryTests: TestFixtureBase
    {
        [Test]
        public void ValueFactoryShouldBeInitializedByStatic()
        {
            Assert.IsTrue(ValueFactory.Initialized,"Value factory should have been initialized by the static constructor");
        }

        [Test]
        public void ShouldHaveMappingForUserTypeAfterAutoConfigure()
        {
            Assert.IsTrue(ValueFactory.Initialized);
            Assert.IsTrue(ConfigurationMapper.HasMappingFor<User>(), "Configuration should have mapping for 'User' class defined in test assembly after autoconfigure");
        }

        [Test]
        public void ShouldGetAnEmptyValueWithEmptyData()
        {
            Dictionary<string, object> dv = new Dictionary<string, object>();
            dv.Add("test_name", "Rui");
            User user = ValueFactory.TryGet<User>(dv);
            IConfigurationMappingsBase<User> mapper = ConfigurationMapper.GetMapper<User>();
            Assert.IsNotNull(mapper);
            Log(mapper.ConfiguredType);
            Log(mapper.Settings.ToList());
            var result = mapper.Get(dv);
            Assert.IsNotNull(result);
            Assert.IsTrue(ConfigurationMapper.HasMappingFor<User>()); 
            Log(result.ToString());
            Log(ConfigurationMapper.HasMappingFor<User>(), "Have mapping for user");
            Assert.IsNotNull(user);
            Log(user.ToString());
            
        }

        [Test]
        public void ShouldCreateDynamicDictionary()
        {
            dynamic values = new DynamicConfigurationValues();
            values["hello"] = "world";

            Assert.AreEqual(values.hello,"world");
        }
    }
}
