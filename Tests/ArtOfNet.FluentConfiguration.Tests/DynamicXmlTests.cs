using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ArtOfNet.ConfArt.Core;
using System.Diagnostics;
using System.Web.Routing;

namespace ConfArtTests
{
    [TestFixture]
    public class DynamicXmlTests: TestFixtureBase
    {
        public DynamicXmlTests()
        {

        }

        [Test]
        public void ShouldLoadDynamicFromXmlString()
        {
            string xmlString = @"<Contacts><User name=""user1""/><User name=""user2""/></Contacts>";
            dynamic xml = new DynamicXmlElement(xmlString);

            dynamic users = xml.User;
            
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count == 2);
            foreach (dynamic user in users)
            {
                Assert.IsNotNull(user.name);
            }
            string user1 = users[0].name;
            Assert.AreEqual(user1, "user1");

            string user2 = users[1].name;
            Assert.AreEqual(user2, "user2");

        }

        [Test]
        public void ShouldUseDynamicXmlAsDynamicModel()
        {
            string xmlString = @"<Model name=""Rui""><site>ArtOfNet</site><child name=""Thaïs""/><child name=""Léandre"" /></Model>";
            dynamic model = new DynamicXmlElement(xmlString);
            
            Assert.AreEqual(model.name,"Rui");
            Assert.AreEqual(model.child.Count,2);
            foreach (dynamic item in model.child)
            {
                Assert.IsNotNull(item.name);
            }
            Assert.AreEqual(model.child[0].name,"Thaïs");
            Assert.AreEqual(model.child[1].name, "Léandre");
        }

        [Test]
        public void ShouldGetValuesFromDynamicXmlRoute()
        {
            string xmlstring = @"
                <add routeName=""default"" path=""{controller}/{action}"">
                    <defaultValues controller=""Home"" action=""Index""/>
                </add>";
            dynamic route = new DynamicXmlElement(xmlstring);

            //Assert.AreEqual(route.Name, "add");
            Assert.AreEqual(route.routeName, "default");
            Assert.AreEqual(route.path, "{controller}/{action}");

            dynamic defaults = route.defaultValues;

            Assert.AreEqual(defaults.controller,"Home");
            var anon = new { controller = "Home", action = "Index" };
            Assert.AreEqual(defaults.controller, anon.controller);

            Dictionary<string, object> dico = new Dictionary<string, object>();
            dico.Add("controller","Home");
            dico.Add("action", "About");

        }

        [Test]
        public void ShouldNotExplodeWithNonExistingKey()
        {
            string xmlstring = @"
                <add routeName=""default"" path=""{controller}/{action}"">
                    <defaultValues controller=""Home"" action=""Index""/>
                </add>";
            dynamic route = new DynamicXmlElement(xmlstring);

            Assert.DoesNotThrow(
                () =>
                {
                    var notExistingNode = route.constraints;
                });
    
        }
    }
}
