using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ConfArtTests.SampleData;
using ConfArt.Core;
using System.Threading;
using ArtOfNet.ConfArt.Core;

namespace ConfArtTests
{
    [TestFixture]
    public class ScenarioTest
    {
        [SetUp]
        public void Setup()
        {
            
        }
        [Test]
        public void VerifyTestContext()
        {
        }

        public void ThisMethodIsNotATest()
        {
            Console.WriteLine("ThisMethodIsNotATest");
        }

        [Test]
        public void ShouldGetNotNullInstanceOfMappedObject()
        {
            Dictionary<string, object> dv = new Dictionary<string, object>();

            User actual = ValueFactory.TryGet<User>(dv);
            Assert.IsNotNull(actual);
        }

        [Test]
        public void ShouldGetCorrectInstanceOfMappedObject()
        {
            Dictionary<string, object> dv = new Dictionary<string, object>();

            dv.Add("test_name", "Rui");
            dv.Add("test_id", 100);
            dv.Add("test_birth", DateTime.Parse("1975/04/24"));

            User actual = ValueFactory.TryGet<User>(dv);

            User expected = new User();
            expected.Name = "Rui";
            expected.Id = 100;
            expected.Birth = DateTime.Parse("1975/04/24");

            Assert.AreEqual(expected.Birth, actual.Birth);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
        }
        [Test]
        public void ShouldGetValidInstanceOfMappedObjectFromDynamicValue()
        {
            dynamic dv = new DynamicConfigurationValues();
            dv.test_name = "Rui";
            dv.test_id = 100;
            dv.test_birth = DateTime.Parse("1975/04/24");

            User actual = ValueFactory.TryGet<User>(dv);
            User expected = UserHelper.GetUserRui();
            Assert.AreEqual(actual.ToString(), expected.ToString());
        }

        [Test]
        public void ShouldGetValidInstanceOfMappedObjectFromDictionary()
        {
            Dictionary<string, object> dv = new Dictionary<string, object>();

            dv.Add("test_name", "Rui");
            dv.Add("test_id", 100);
            dv.Add("test_birth", DateTime.Parse("1975/04/24"));

            User actual = ValueFactory.TryGet<User>(dv);
            User expected = UserHelper.GetUserRui();

            Assert.AreEqual(actual.ToString(), expected.ToString());
        }

        [Test]
        public void ShouldGetValidInstanceEvenIfNotAllMembersMatching()
        {
            Dictionary<string, object> dv = new Dictionary<string, object>();

            dv.Add("test_name", "Rui");
            dv.Add("test_id", 100);
            dv.Add("test_xxx", DateTime.Parse("1975/04/24"));

            User actual = ValueFactory.TryGet<User>(dv);
            User expected = UserHelper.GetUserRui();
            //set birth date of expected to 0 to simulate a non-matching element:
            expected.Birth = new DateTime();

            Assert.AreEqual(actual.ToString(), expected.ToString());
        }

        [Test(Description="With auto mapping feature, it should try to fill as best as possible the activated instance")]
        public void ShouldGetAValueBasedOnMemberNamesWithoutMapping()
        {
            dynamic dv = new DynamicConfigurationValues();
            dv.name = "Rui";
            dv.id = 111;

            Contact expected = new Contact();
            expected.Name = "Rui";
            expected.Id = 111;

            Contact actual = ValueFactory.TryGet<Contact>(dv);
            Assert.AreEqual(actual, expected);
        }

        [Test(Description="Mapping a an xml string to a real object")]
        public void ShouldMapADynamicXmlObjectToARealImplementation()
        {
            string xmlstring = @"
                <Contact Name=""Rui"" Id=""111"" />";
            dynamic contactDynamic = new DynamicXmlElement(xmlstring);


            Contact expected = new Contact();
            expected.Name = "Rui";
            expected.Id = 111;

            Contact actual = ValueFactory.TryGet<Contact>(contactDynamic);
            Assert.AreEqual(actual, expected);
        }

        [Test(Description = "Mapping a an xml string to a real object with nested elements")]
        public void ShouldMapADynamicXmlObjectToClassWithNestedElements()
        {
            string xmlstring = @"
                <Contact Name=""Rui"" Id=""111"">
                    <phone>1234567890</phone>
                    <mail>rui@somewhere.com</mail>
                    <cities>
                        <city>Paris</city>
                        <city>Lisbon</city>
                    </cities>
                </Contact>
                ";
            dynamic contactDynamic = new DynamicXmlElement(xmlstring);

            Contact expected = new Contact();
            expected.Name = "Rui";
            expected.Id = 111;
            expected.Mail = "rui@somewhere.com";
            expected.Phone = "1234567890";
            expected.Cities.Add("Paris");
            expected.Cities.Add("Lisbon");
            Contact actual = ValueFactory.TryGet<Contact>(contactDynamic);
            Assert.AreEqual(actual, expected);
        }
    }
}
